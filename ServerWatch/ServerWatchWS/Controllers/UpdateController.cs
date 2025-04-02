using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ServerWatchWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly string downloadUrl = "http://192.168.1.138:5000/downloads/ServerWatchAgent.exe";

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

        private async Task<Version> GetAvailableVersionAsync(string fileUrl)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), "ServerWatchAgent.exe");

            using (HttpClient client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(fileUrl);
                await System.IO.File.WriteAllBytesAsync(tempFilePath, bytes);
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(tempFilePath);
            var version = new Version(fileVersionInfo.FileVersion);

            System.IO.File.Delete(tempFilePath);

            return version;
        }
    }
}
