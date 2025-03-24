using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerWatchAgent
{
    public class DataSender
    {
        public async Task SendMirroringDataAsync(string jsonPayload)
        {
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:5000");

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
