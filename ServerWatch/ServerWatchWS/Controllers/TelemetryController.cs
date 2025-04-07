using CosysLib.ExceptionManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ServerWatchWS.Data;
using ServerWatchWS.Model;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

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

            _context.DriverEntries.AddRange(entries!);
            await _context.SaveChangesAsync();

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

            _context.MirroringEntries.AddRange(entries!);
            await _context.SaveChangesAsync();

            return Ok("Data inserted successfully.");
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

            var server = await _context.Servers.FirstOrDefaultAsync(s => s.GUID == guid);
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
