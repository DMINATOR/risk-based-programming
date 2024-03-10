using Examples.JSON;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ExampleTests.Integration
{
    public class DownloadJsonTests
    {
        ServiceProvider _serviceProvider;
        private readonly ITestOutputHelper _output;

        // Values
        private string EndpointUrlSource = "https://meowfacts.herokuapp.com/"; // Random cat fact
        private string DownloadFileName = $"randomcatfact-{Guid.NewGuid()}.json";

        public DownloadJsonTests(ITestOutputHelper output)
        {
            _output = output;

            var services = new ServiceCollection();
            services.AddHttpClient();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task StartupCreditCardIssuerFixedTests_IssueCard_Default_Success()
        {
            // Arrange
            var download = new DownloadJson(_serviceProvider.GetRequiredService<IHttpClientFactory>());

            // Act
            var directory = Directory.CreateTempSubdirectory();
            var path = Path.Combine(directory.FullName, DownloadFileName);
            await download.DownloadAsync(EndpointUrlSource, path);

            // Assert
            _output.WriteLine($"Stored file '{EndpointUrlSource}' -> '{path}'");

            var contents = File.ReadAllText(path);
            _output.WriteLine($"'{contents}'");

            Assert.Contains("data", contents);
        }
    }
}
