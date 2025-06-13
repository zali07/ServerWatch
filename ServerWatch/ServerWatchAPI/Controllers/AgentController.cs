using Newtonsoft.Json;
using ServerWatchAPI.Data;
using ServerWatchAPI.Model;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerWatchAPI.Controllers
{
    [RoutePrefix("Agent")]
    public class AgentController : ApiController
    {
        private readonly DataLayer dataLayer;
        private readonly string downloadUrl;

        public AgentController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            downloadUrl = ConfigurationManager.AppSettings["UpdatePath"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string not configured.");
            }

            if (string.IsNullOrWhiteSpace(downloadUrl))
            {
                throw new InvalidOperationException("Update path is not configured.");
            }

            dataLayer = new DataLayer(connectionString);
        }

        [HttpGet]
        [Route("getAgentUpdateInfo")]
        public async Task<IHttpActionResult> GetUpdateInfo()
        {
            try
            {
                var availableVersion = await GetAvailableVersionAsync(downloadUrl);

                var updateInfo = new
                {
                    Version = availableVersion.ToString(),
                    DownloadUrl = downloadUrl
                };

                return Ok(updateInfo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private static async Task<Version> GetAvailableVersionAsync(string fileUrl)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), "ServerWatchAgent.exe");

            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(fileUrl, tempFilePath);
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(tempFilePath);
            var versionString = fileVersionInfo.FileVersion;

            if (string.IsNullOrEmpty(versionString))
            {
                throw new InvalidOperationException("File version information is missing.");
            }

            var version = new Version(versionString);
            File.Delete(tempFilePath);

            return version;
        }

        [HttpPost]
        [Route("registerAgent")]
        public async Task<IHttpActionResult> Register()
        {
            string body = await Request.Content.ReadAsStringAsync();

            ServerRegistration serverToRegister;
            try
            {
                serverToRegister = JsonConvert.DeserializeObject<ServerRegistration>(body);
            }
            catch
            {
                return BadRequest("Invalid JSON.");
            }

            if (serverToRegister == null || string.IsNullOrWhiteSpace(serverToRegister.PublicKey) || string.IsNullOrWhiteSpace(serverToRegister.GUID))
            {
                return BadRequest("Data is null or incomplete.");
            }

            var existingServer = await dataLayer.GetServerByGUID(serverToRegister.GUID);
            if (existingServer != null)
            {
                return Ok("Server already registered.");
            }

            await dataLayer.RegisterServerAsync(serverToRegister.GUID, serverToRegister.PublicKey);

            return Ok("Server registered successfully.");
        }

        [HttpGet]
        [Route("getServerStatus")]
        public async Task<IHttpActionResult> GetAgentStatus()
        {
            var guidHeader = Request.Headers.Contains("ServerGuid") ? Request.Headers.GetValues("ServerGuid") : null;
            var guid = guidHeader?.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Content(HttpStatusCode.Unauthorized, "Missing server guid.");
            }

            try
            {
                var agent = await dataLayer.GetServerByGUID(guid);

                if (agent == null)
                {
                    return Ok(new
                    {
                        approved = false,
                        message = "Agent with the specified guid was not found. Please register it first."
                    });
                }

                return Ok(new { approved = ((agent.Flag & 1) == 1) });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
