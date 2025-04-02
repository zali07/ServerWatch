using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServerWatchWS.Data;
using ServerWatchWS.Model;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        private readonly AppDbContext _context;

        public DriverController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("postDriverData")]
        public async Task<IActionResult> PostData()
        {
            string? signature = Request.Headers["X-CosysKey"];

            if (string.IsNullOrWhiteSpace(signature))
            {
                return Unauthorized("Missing signature.");
            }

            string body;
            using (var reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            List<DriverData>? entries;

            try
            {
                entries = JsonConvert.DeserializeObject<List<DriverData>>(body);
            }
            catch
            {
                return BadRequest("Invalid JSON.");
            }

            if (entries == null || entries.Count == 0)
            {
                return BadRequest("Data is null");
            }

            string? guid = Request.Headers["ServerGuid"];

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Unauthorized("Missing server guid.");
            }

            var server = await _context.Servers.FirstOrDefaultAsync(s => s.GUID == guid);

            if (server == null)
            {
                return Unauthorized("Unknown agent.");
            }

            if (!SignatureHelper.VerifyPayload(body, signature, server.PublicKey))
            {
                return Unauthorized("Invalid signature.");
            }

            _context.DriverEntries.AddRange(entries);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
