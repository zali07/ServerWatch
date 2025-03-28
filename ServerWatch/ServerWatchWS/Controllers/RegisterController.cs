using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWatchWS.Data;
using ServerWatchWS.Model;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly AppDbContext _context;

        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("registerNewServer")]
        public async Task<IActionResult> Register([FromBody] ServerRegistration serverToRegister)
        {
            if (serverToRegister == null)
            {
                return BadRequest("Data is null");
            }

            var existingServer = await _context.Servers.FirstOrDefaultAsync(s => s.GUID == serverToRegister.GUID);

            if (existingServer != null)
            {
                return Ok("Server already registered.");
            }

            var server = new Servers
            {
                GUID = serverToRegister.GUID,
                PublicKey = serverToRegister.PublicKey,
                IsApproved = false
            };

            _context.Servers.Add(server);

            await _context.SaveChangesAsync();

            return Ok("Server registered successfully.");
        }

        [HttpGet("getServerStatus")]
        public async Task<IActionResult> GetAgentStatus(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                return BadRequest("GUID is required.");
            }

            try
            {
                var agent = await _context.Servers.FirstOrDefaultAsync(x => x.GUID == guid);

                if (agent == null)
                {
                    return Ok(new { approved = false, message = "Agent with the specified guid was not found. " +
                                                                "Please register it first." });
                }

                return Ok(new { approved = agent.IsApproved });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}
