using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerWatchWS.Data;
using ServerWatchWS.Model;
using System.Diagnostics;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController : Controller
    {
        private readonly DataLayer _dataLayer;
        private readonly string downloadUrl;

        public AgentController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _dataLayer = new DataLayer(connectionString!);
            downloadUrl = configuration.GetValue<string>("UpdatePath")!;

            if (string.IsNullOrEmpty(downloadUrl))
            {
                throw new InvalidOperationException("Update path is not configured.");
            }
        }

        [HttpGet("getAgentUpdateInfo")]
        public async Task<IActionResult> GetUpdateInfo()
        {
            var availableVersion1 = await GetAvailableVersionAsync(downloadUrl);

            var updateInfo = new
            {
                Version = availableVersion1,
                DownloadUrl = downloadUrl
            };

            return Ok(updateInfo);
        }

        private static async Task<Version> GetAvailableVersionAsync(string fileUrl)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), "ServerWatchAgent.exe");

            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(fileUrl);
                await System.IO.File.WriteAllBytesAsync(tempFilePath, bytes);
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(tempFilePath);
            var versionString = fileVersionInfo.FileVersion;

            if (string.IsNullOrEmpty(versionString))
            {
                throw new InvalidOperationException("File version information is missing.");
            }

            var version = new Version(versionString);

            System.IO.File.Delete(tempFilePath);

            return version;
        }

        [HttpPost("registerAgent")]
        public async Task<IActionResult> Register()
        {
            string body;
            using (var reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            ServerRegistration? serverToRegister;
            try
            {
                serverToRegister = JsonConvert.DeserializeObject<ServerRegistration>(body);
            }
            catch
            {
                return BadRequest("Invalid JSON.");
            }

            if (serverToRegister == null || serverToRegister.PublicKey == null || serverToRegister.GUID == null)
            {
                return BadRequest("Data is null");
            }

            var existingServer = await _dataLayer.GetServerByGUID(serverToRegister.GUID);

            if (existingServer != null)
            {
                return Ok("Server already registered.");
            }

            await _dataLayer.RegisterServerAsync(serverToRegister.GUID, serverToRegister.PublicKey);

            return Ok("Server registered successfully.");
        }

        [HttpGet("getServerStatus")]
        public async Task<IActionResult> GetAgentStatus()
        {
            string? guid = Request.Headers["ServerGuid"];

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Unauthorized("Missing server guid.");
            }

            try
            {
                var agent = await _dataLayer.GetServerByGUID(guid);

                if (agent == null)
                {
                    return Ok(new
                    {
                        approved = false,
                        message = "Agent with the specified guid was not found. Please register it first."
                    });
                }

                return Ok(new { approved = ((agent.Flag & 1) == 1) /* IsApproved */ });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex });
            }
        }
    }
}
