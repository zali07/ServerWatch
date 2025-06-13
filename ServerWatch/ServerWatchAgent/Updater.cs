using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ServerWatchAgent
{
    public class Updater
    {
        public async Task GetUpdateInfoAsync()
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseApiUrl"]);

                HttpResponseMessage response = await client.GetAsync("/Cosys.ServerWatch/Agent/getAgentUpdateInfo");

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }

                string jsonPayload = await response.Content.ReadAsStringAsync();

                await CheckForUpdates(jsonPayload);
            }
        }

        private async Task CheckForUpdates(string jsonPayload)
        {
            try
            {
                var updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(jsonPayload);
                
                if (updateInfo == null)
                {
                    throw new Exception("Unable to parse update info.");
                }

                string newExePath = updateInfo.DownloadUrl;
                var availableVersion = new Version(updateInfo.Version);

                string oldExePath = Assembly.GetExecutingAssembly().Location;
                var currentVersion = new Version(FileVersionInfo.GetVersionInfo(oldExePath).FileVersion);

                if (currentVersion.CompareTo(availableVersion) < 0)
                {
                    string newExeLocalPath = await DownloadUpdateAsync(updateInfo.DownloadUrl);

                    StartUpdaterProcess("ServerWatchAgent", newExeLocalPath, oldExePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking for updates.", ex);
            }
        }

        private void StartUpdaterProcess(string serviceName, string newExePath, string oldExePath)
        {
            var updaterExePath = @"C:\Program Files (x86)\Cosys\ServerWatch\ServerWatchAgentUpdater.exe";

            // arguments:  <serviceName> <newExePath> <targetExePath>
            var processInfo = new ProcessStartInfo
            {
                FileName = updaterExePath,
                Arguments = $"\"{serviceName}\" \"{newExePath}\" \"{oldExePath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(processInfo);
        }

        private async Task<string> DownloadUpdateAsync(string downloadUrl)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "ServerWatchAgent.exe");

            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }

            return tempFilePath;
        }

        public class UpdateInfo
        {
            public string Version { get; set; }
            public string DownloadUrl { get; set; }
        }
    }
}
