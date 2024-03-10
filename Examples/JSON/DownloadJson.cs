using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Examples.JSON
{
    public class DownloadJson
    {
        private IHttpClientFactory _clientFactory;

        public DownloadJson(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task DownloadAsync(string endpointUrl, string outputPath)
        {
            var httpClient = _clientFactory.CreateClient();

            try
            {
                // Download JSON data from the API
                string jsonData = await httpClient.GetStringAsync(endpointUrl);

                // Save the JSON data to a file
                File.WriteAllText(outputPath, jsonData);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error downloading JSON: {ex.Message}");
            }
        }
    }
}
