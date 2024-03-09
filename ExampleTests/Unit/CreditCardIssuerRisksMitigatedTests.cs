using Examples.CreditCard.RisksMitigated;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace ExampleTests.Unit
{
    public class CreditCardIssuerRisksMitigatedTests
    {
        private TimeProvider _timeProvider;

        private Mock<ICreditCardNumberGenerator> _creditCardNumberGeneratorMock;
        private ICreditCardNumberGenerator _creditCardNumberGenerator;

        private Mock<ICreditCardCVCGenerator> _creditCardCVCGeneratorMock;
        private ICreditCardCVCGenerator _creditCardCVCGenerator;

        public CreditCardIssuerRisksMitigatedTests()
        {
            _timeProvider = new FakeTimeProvider();
        }

        private void SetupMocks()
        {
            var cardNumber = "4242424242424242";

            _creditCardNumberGeneratorMock = new Mock<ICreditCardNumberGenerator>();
            _creditCardNumberGeneratorMock.Setup(m => m.Generate()).Returns(cardNumber);
            _creditCardNumberGenerator = _creditCardNumberGeneratorMock.Object;

            _creditCardCVCGeneratorMock = new Mock<ICreditCardCVCGenerator>();
            _creditCardCVCGeneratorMock.Setup(m => m.Generate(cardNumber)).Returns("123");
            _creditCardCVCGenerator = _creditCardCVCGeneratorMock.Object;
        }

        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_Default_Success()
        {
            // Arrange
            var firstName = "First";
            var lastName = "Last";
            SetupMocks();
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // Act
            var card = issuer.IssueCard(firstName, lastName);

            // Assert
            _creditCardCVCGeneratorMock.VerifyAll();
            _creditCardNumberGeneratorMock.VerifyAll();
            Assert.Equal(firstName, card.FirstName);
            Assert.Equal(lastName, card.LastName);
        }
    }
}