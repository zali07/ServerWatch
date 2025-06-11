using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServerWatchWS.Data;
using ServerWatchWS.Model;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : Controller
    {
        private readonly DataLayer _dataLayer;

        public TelemetryController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _dataLayer = new DataLayer(connectionString!);
        }

        [HttpPost("postDriverData")]
        public async Task<IActionResult> PostDriverData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<DriverData>();
            if (result != null) return result;

            foreach (var entry in entries!)
            {
                entry.ServerGUID = guid!;
                entry.TS = DateTime.UtcNow;
            }

            await _dataLayer.InsertDriverEntriesAsync(entries!);

            return Ok();
        }

        [HttpPost("postMirroringData")]
        public async Task<IActionResult> PostMirroringData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<MirroringData>();
            if (result != null) return result;

            foreach (var entry in entries!)
            {
                entry.ServerGUID = guid!;
                entry.TS = DateTime.UtcNow;
            }

            await _dataLayer.InsertMirroringEntriesAsync(entries!);

            return Ok();
        }

        [HttpPost("postBackupData")]
        public async Task<IActionResult> PostBackupData()
        {
            var (result, entries, guid) = await AuthenticateAndParseRequest<BackupData>();
            if (result != null) return result;

            foreach (var entry in entries!)
            {
                entry.ServerGUID = guid!;
                entry.TS = DateTime.UtcNow;
            }

            await _dataLayer.InsertBackupEntriesAsync(entries!);

            return Ok();
        }

        [HttpGet("getBackupConfig")]
        public async Task<IActionResult> GetBackupConfig()
        {
            string? guid = Request.Headers["ServerGuid"];

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Unauthorized("Missing server guid.");
            }

            try
            {
                var backupRootFolder = await _dataLayer.GetBackupRootFolderAsync(guid!);

                if (string.IsNullOrWhiteSpace(backupRootFolder))
                {
                    return Ok(new
                    {
                        backupRootFolder = "C:\\Backups" // Default backup root folder if not set
                    });
                }

                return Ok(new { backupRootFolder });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        private async Task<(IActionResult? result, List<T>? data, string? guid)> AuthenticateAndParseRequest<T>() where T : class
        {
            string? signature = Request.Headers["X-CosysKey"];
            if (string.IsNullOrWhiteSpace(signature))
            {
                return (Unauthorized("Missing signature."), null, null);
            }

            string body;
            using (var reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            List<T>? entries;
            try
            {
                entries = JsonConvert.DeserializeObject<List<T>>(body);
            }
            catch
            {
                return (BadRequest("Invalid JSON."), null, null);
            }

            if (entries == null || entries.Count == 0)
            {
                return (BadRequest("Data is null"), null, null);
            }

            string? guid = Request.Headers["ServerGuid"];
            if (string.IsNullOrWhiteSpace(guid))
            {
                return (Unauthorized("Missing server guid."), null, null);
            }

            var server = await _dataLayer.GetServerByGUID(guid);

            if (server == null)
            {
                return (Unauthorized("Unknown agent."), null, null);
            }

            if (!SignatureHelper.VerifyPayload(body, signature, server.PublicKey))
            {
                return (Unauthorized("Invalid signature."), null, null);
            }

            return (null, entries, guid);
        }
    }
}
