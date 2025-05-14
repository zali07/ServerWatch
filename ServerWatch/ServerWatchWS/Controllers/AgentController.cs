using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWatchWS.Data;
using ServerWatchWS.Model;
using System.Diagnostics;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        private readonly string downloadUrl = "http://192.168.1.128:5000/downloads/ServerWatchAgent.exe";

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
        public async Task<IActionResult> GetAgentStatus()
        {
            string? guid = Request.Headers["ServerGuid"];

            if (string.IsNullOrWhiteSpace(guid))
            {
                return Unauthorized("Missing server guid.");
            }

            try
            {
                var agent = await _context.Servers.FirstOrDefaultAsync(x => x.GUID == guid);

                if (agent == null)
                {
                    return Ok(new
                    {
                        approved = false,
                        message = "Agent with the specified guid was not found. " +
                                                                "Please register it first."
                    });
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
