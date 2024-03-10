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

        [Fact]
        public void StartupCreditCardIssuerFixedTests_IssueCard_Default_Success()
        {
            // Arrange
            var now = DateTime.UtcNow;

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