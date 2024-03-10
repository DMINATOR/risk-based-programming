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

                #region Http Request risks
                // ❗ Potential issues:
                // 1 - endpoint stops working:
                //      - temporary or permanently unavailable (500 errors)
                //      - throttling (429)
                //      - authentication fails (secrets/keys/certs can expire)
                //      - general network connection issues: dns name changes, or packet loss
                // 2 - unexpected change to JSON file structure by owner, results in a different file format when saved
                // 3 - unexpected behaviours:
                //      - consider a redirect can happen (302,303,307), and download code is not handling this correctly
                //      - consider that instead of receiving (200 OK) other return codes are returned, resulting in an HTML content to be returned instead of JSON

                #endregion

                // Save the JSON data to a file
                File.WriteAllText(outputPath, jsonData);

                #region File Storage risks
                // ❗ Potential issues:
                // 1 - Location is unabtainable, ie path doesn't exist or doesn't have permissions to write there
                // 2 - File is not named uniquely, can be overriden with the next request
                // 3 - Multiple threads writing to the same file, potential corruption
                // 4 - Running out of space to store a file
                // 5 - File is corrupted (partially stored)
                #endregion
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error downloading JSON: {ex.Message}");
            }
        }
    }
}
