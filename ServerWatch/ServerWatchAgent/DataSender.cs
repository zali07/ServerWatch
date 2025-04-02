using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerWatchAgent
{
    public class DataSender
    {
        private readonly string baseUrl = "http://192.168.1.138:5000";

        public async Task SendMirroringDataAsync(string jsonPayload)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(baseUrl);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/mirroring/postMirroringData", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }
            }
        }

        public async Task SendDriverDataAsync(string jsonPayload)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(baseUrl);

                var signature = KeyContainerManager.SignData(jsonPayload);

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/driver/postDriverData")
                {
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
                };

                request.Headers.Add("X-CosysKey", signature);
                request.Headers.Add("ServerGuid", KeyContainerManager.Guid);

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }
            }
        }

        public async Task RegisterWithWebServiceAsync(string jsonPayload)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(baseUrl);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/register/registerNewServer", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }
            }
        }

        public async Task<bool> CheckApprovalStatusAsync(string guid)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(baseUrl);

                HttpResponseMessage response = await client.GetAsync($"/api/register/getServerStatus?guid={guid}");

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                return responseContent.Contains("approved\":true");
            }
        }

        public async Task SendDriverDataAsync(string jsonPayload)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:5000");

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/driver/postDriverData", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }
            }
        }
    }
}
