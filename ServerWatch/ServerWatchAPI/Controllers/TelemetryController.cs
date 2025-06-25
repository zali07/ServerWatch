using Newtonsoft.Json;
using ServerWatchAPI.Data;
using ServerWatchAPI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerWatchAPI.Controllers
{
    [RoutePrefix("Telemetry")]
    public class TelemetryController : ApiController
    {
        private readonly DataLayer dataLayer;

        public TelemetryController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string not configured.");
            }

            dataLayer = new DataLayer(connectionString);
        }

        [HttpPost]
        [Route("postDriverData")]
        public async Task<IHttpActionResult> PostDriverData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<DriverData>();
            if (result != null) return result;

            foreach (var entry in entries)
            {
                entry.ServerGUID = guid;
                entry.TS = DateTime.UtcNow;
            }

            try
            {
                await dataLayer.InsertDriverEntriesAsync(entries);
                return Ok();
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

        [HttpPost]
        [Route("postMirroringData")]
        public async Task<IHttpActionResult> PostMirroringData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<MirroringData>();
            if (result != null) return result;

            foreach (var entry in entries)
            {
                entry.ServerGUID = guid;
                entry.TS = DateTime.UtcNow;
            }

            try
            {
                await dataLayer.InsertMirroringEntriesAsync(entries);
                return Ok();
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

        [HttpPost]
        [Route("postBackupData")]
        public async Task<IHttpActionResult> PostBackupData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<BackupData>();
            if (result != null) return result;

            foreach (var entry in entries)
            {
                entry.ServerGUID = guid;
                entry.TS = DateTime.UtcNow;
            }

            try
            {
                await dataLayer.InsertBackupEntriesAsync(entries);
                return Ok();
            }catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet]
        [Route("getMirroringStatus")]
        public Task<IHttpActionResult> GetMirroringStatus() => GetApprovalStatus(4); // Bit 2

        [HttpGet]
        [Route("getDriverStatus")]
        public Task<IHttpActionResult> GetDriverStatus() => GetApprovalStatus(8); // Bit 3

        [HttpGet]
        [Route("getBackupStatus")]
        public Task<IHttpActionResult> GetBackupStatus() => GetApprovalStatus(16); // Bit 4

        private async Task<IHttpActionResult> GetApprovalStatus(int flagBit)
        {
            var guid = GetHeaderValue("ServerGuid");

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

                bool isApproved = (agent.Flag & flagBit) == flagBit;

                return Ok(new { approved = isApproved });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("getBackupConfig")]
        public async Task<IHttpActionResult> GetBackupConfig()
        {
            var guid = GetHeaderValue("ServerGuid");

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Content(HttpStatusCode.Unauthorized, "Missing server guid.");
            }

            try
            {
                var backupRootFolder = await dataLayer.GetBackupRootFolderAsync(guid);

                return Ok(new
                {
                    backupRootFolder = string.IsNullOrWhiteSpace(backupRootFolder)
                        ? @"C:\Backups"
                        : backupRootFolder
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private async Task<(IHttpActionResult result, List<T> data, string guid)> AuthenticateAndParseRequest<T>() where T : class
        {
            var signature = GetHeaderValue("X-CosysKey");
            if (string.IsNullOrWhiteSpace(signature))
            {
                return (Content(HttpStatusCode.Unauthorized, "Missing signature."), null, null);
            }

            var body = await Request.Content.ReadAsStringAsync();

            List<T> entries;
            try
            {
                entries = JsonConvert.DeserializeObject<List<T>>(body);

                if (entries == null || entries.Count == 0)
                {
                    T single = JsonConvert.DeserializeObject<T>(body);
                    if (single != null)
                    {
                        entries = new List<T> { single };
                    }
                }
            }
            catch
            {
                return (BadRequest("Invalid JSON."), null, null);
            }

            if (entries == null || entries.Count == 0)
            {
                return (BadRequest("Data is null"), null, null);
            }

            var guid = GetHeaderValue("ServerGuid");
            if (string.IsNullOrWhiteSpace(guid))
            {
                return (Content(HttpStatusCode.Unauthorized, "Missing server guid."), null, null);
            }

            var server = await dataLayer.GetServerByGUID(guid);
            if (server == null)
            {
                return (Content(HttpStatusCode.Unauthorized, "Unknown agent."), null, null);
            }

            if (!SignatureHelper.VerifyPayload(body, signature, server.PublicKey))
            {
                return (Content(HttpStatusCode.Unauthorized, "Invalid signature."), null, null);
            }

            return (null, entries, guid);
        }

        private string GetHeaderValue(string key)
        {
            if (Request.Headers.Contains(key))
            {
                return Request.Headers.GetValues(key).FirstOrDefault();
            }

            return null;
        }
    }
}
