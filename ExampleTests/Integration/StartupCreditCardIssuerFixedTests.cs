using Microsoft.Extensions.DependencyInjection;

namespace ExampleTests.Integration
{
    public class StartupCreditCardIssuerFixedTests
    {
        ServiceProvider _serviceProvider;

        public StartupCreditCardIssuerFixedTests()
        {
            var services = new ServiceCollection();
            StartupCardIssuerFixed.ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        [Theory]
        [InlineData(10)]
        public void StartupCreditCardIssuerFixedTests_IssueCard_Default_Success(int iterations)
        {
            // Arrange
            var now = DateTime.UtcNow;

            for(var i = 0; i < iterations; i++)
            {
                // Act
                var card = StartupCardIssuerFixed.IssueCard(_serviceProvider);

                // Assert
                Assert.Equal("name1", card.FirstName);
                Assert.Equal("name2", card.LastName);
                Assert.Equal(16, card.Number.Length);
                Assert.Equal(3, card.CVC.Length);
                Assert.Equal(now.Month, card.ValidMonth);
                Assert.Equal(now.Year + 5, card.ValidYear); // with default validity years
            }
        }
    }
}