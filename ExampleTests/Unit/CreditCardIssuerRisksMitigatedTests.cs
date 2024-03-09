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


        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_Constructor_TimeProvider_IsNull_ThrowsException()
        {
            // Arrange
            // Act
            SetupMocks();
            var ex = Assert.ThrowsAny<Exception>(() =>
                 new CreditCardIssuerRisksMitigated(null, _creditCardNumberGenerator, _creditCardCVCGenerator)
            );

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'timeProvider')", ex.Message);
        }

        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_Constructor_NumberGenerator_IsNull_ThrowsException()
        {
            // Arrange
            // Act
            SetupMocks();
            var ex = Assert.ThrowsAny<Exception>(() =>
                 new CreditCardIssuerRisksMitigated(_timeProvider, null, _creditCardCVCGenerator)
            );

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'numberGenerator')", ex.Message);
        }

        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_Constructor_CVCGenerator_IsNull_ThrowsException()
        {
            // Arrange
            // Act
            SetupMocks();
            var ex = Assert.ThrowsAny<Exception>(() =>
                 new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, null)
            );

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'cvcGenerator')", ex.Message);
        }

        [Theory]
        [InlineData(-2, "validityYears (Parameter 'Provided validity years is outside expected range 0..99')")]
        [InlineData(-1, "validityYears (Parameter 'Provided validity years is outside expected range 0..99')")]
        [InlineData(100, "validityYears (Parameter 'Provided validity years is outside expected range 0..99')")]
        [InlineData(101, "validityYears (Parameter 'Provided validity years is outside expected range 0..99')")]
        public void CreditCardIssuerRisksMitigatedTests_Constructor_ValidityYears_Invalid_ThrowsException(int validityYears, string expectedException)
        {
            // Arrange
            // Act
            SetupMocks();
            var ex = Assert.ThrowsAny<Exception>(() =>
                 new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator, validityYears)
            );

            // Assert
            Assert.Equal(expectedException, ex.Message);
        }
    }
}