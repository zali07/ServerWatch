using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PostData([FromBody] List<DriverData> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                return BadRequest("Data is null");
            }

            _context.DriverEntries.AddRange(entries);

            await _context.SaveChangesAsync();

            return Ok("Data inserted successfully.");
        }
    }
}
