using Examples.CreditCard.RisksMitigated;
using Microsoft.Extensions.Time.Testing;

namespace ExampleTests.Unit
{
    public class CreditCardIssuerRisksMitigatedTests
    {
        private TimeProvider _timeProvider;
        private ICreditCardNumberGenerator _creditCardNumberGenerator;
        private ICreditCardCVCGenerator _creditCardCVCGenerator;

        public CreditCardIssuerRisksMitigatedTests()
        {
            _timeProvider = new FakeTimeProvider();
        }

        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_Default_Success()
        {
            // Arrange
            var firstName = "First";
            var lastName = "Last";
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // Act
            var card = issuer.IssueCard(firstName, lastName);

            // Assert
            Assert.Equal(firstName, card.FirstName);
            Assert.Equal(lastName, card.LastName);
        }
    }
}