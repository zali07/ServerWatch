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
                //client.BaseAddress = new Uri("https://myurl.com");

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/postMirroringData", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP Error: {response.StatusCode}, {error}");
                }
            }
        }
    }
}
