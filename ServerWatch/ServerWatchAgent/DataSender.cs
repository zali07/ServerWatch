using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerWatchAgent
{
    public class DataSender
    {
        private async Task<string> SendRequestAsync(string jsonPayload, string url, HttpMethod method, bool signData = true)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseApiUrl"]);

                var request = new HttpRequestMessage(method, url);

                if (method == HttpMethod.Post && jsonPayload != null)
                {
                    request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    if (signData)
                    {
                        var signature = KeyContainerManager.SignData(jsonPayload);
                        request.Headers.Add("X-CosysKey", signature);
                    }
                }

                request.Headers.Add("ServerGuid", KeyContainerManager.Guid);

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task SendMirroringDataAsync(string jsonPayload)
        {
            await SendRequestAsync(jsonPayload, "/api/telemetry/postMirroringData", HttpMethod.Post);
        }

        public async Task SendDriverDataAsync(string jsonPayload)
        {
            await SendRequestAsync(jsonPayload, "/api/telemetry/postDriverData", HttpMethod.Post);
        }

        public async Task SendBackupDataAsync(string jsonPayload)
        {
            await SendRequestAsync(jsonPayload, "/api/telemetry/postBackupData", HttpMethod.Post);
        }

        public async Task RegisterWithWebServiceAsync(string jsonPayload)
        {
            await SendRequestAsync(jsonPayload, "/api/agent/registerAgent", HttpMethod.Post, false);
        }

        public async Task<bool> CheckApprovalStatusAsync()
        {
            string responseContent = await SendRequestAsync(null, "/api/agent/getServerStatus", HttpMethod.Get);
            return responseContent.Contains("approved\":true");
        }

        public async Task<string> GetBackupFolderPathAsync()
        {
            // { "backupRootFolder": "c:\\Backups" }
            string response = await SendRequestAsync(null, "/api/telemetry/getBackupConfig", HttpMethod.Get);

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new Exception("No response received for backup folder path.");
            }

            try
            {
                var obj = JsonConvert.DeserializeObject<BackupFolderResponse>(response);

                if (obj == null || string.IsNullOrWhiteSpace(obj.backupRootFolder))
                {
                    throw new Exception("Invalid backup folder path response.");
                }

                return obj.backupRootFolder;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse backup folder path response.", ex);
            }
        }

        private class BackupFolderResponse
        {
            public string backupRootFolder { get; set; }
        }
    }
}
