using Examples.CreditCard.RisksMitigated;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace ExampleTests.Unit
{
    public class CreditCardIssuerRisksMitigatedTests
    {
        private FakeTimeProvider _timeProvider;

        private Mock<ICreditCardNumberGenerator> _creditCardNumberGeneratorMock;
        private ICreditCardNumberGenerator _creditCardNumberGenerator;

        private Mock<ICreditCardCVCGenerator> _creditCardCVCGeneratorMock;
        private ICreditCardCVCGenerator _creditCardCVCGenerator;

        // Values
        private string MockedFirstName = "First";
        private string MockedLastName = "Last";
        private string MockedCardNumber = "4242424242424242";
        private string MockedCVC = "123";

        public CreditCardIssuerRisksMitigatedTests()
        {
            _timeProvider = new FakeTimeProvider();
        }

        private void SetupMocks()
        {
            _creditCardNumberGeneratorMock = new Mock<ICreditCardNumberGenerator>();
            _creditCardNumberGeneratorMock.Setup(m => m.Generate()).Returns(MockedCardNumber);
            _creditCardNumberGenerator = _creditCardNumberGeneratorMock.Object;

            _creditCardCVCGeneratorMock = new Mock<ICreditCardCVCGenerator>();
            _creditCardCVCGeneratorMock.Setup(m => m.Generate(MockedCardNumber)).Returns(MockedCVC);
            _creditCardCVCGenerator = _creditCardCVCGeneratorMock.Object;
        }

        [Theory]
        [InlineData(2024, 1, 1)]
        [InlineData(2024, 2, 28)]
        [InlineData(2024, 2, 29)] // Leap day
        [InlineData(2024, 12, 31)]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_Default_Success(int year, int month, int day)
        {
            // Arrange
            SetupMocks();
            var mockedDateTime = new DateTimeOffset(year, month, day, 12, 0, 15, TimeSpan.Zero);
            _timeProvider.SetUtcNow(mockedDateTime);
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // Act
            var card = issuer.IssueCard(MockedFirstName, MockedLastName);

            // Assert
            _creditCardCVCGeneratorMock.VerifyAll();
            _creditCardNumberGeneratorMock.VerifyAll();

            Assert.Equal(MockedFirstName, card.FirstName);
            Assert.Equal(MockedLastName, card.LastName);
            Assert.Equal(MockedCardNumber, card.Number);
            Assert.Equal(MockedCVC, card.CVC);
            Assert.Equal(mockedDateTime.Month, card.ValidMonth);
            Assert.Equal(mockedDateTime.Year + 5, card.ValidYear); // with default validity years
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


        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_NumberGenerator_ThrowsException()
        {
            // Arrange
            SetupMocks();
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // override
            _creditCardNumberGeneratorMock.Setup(m => m.Generate()).Throws(new Exception("Mock Exception"));

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => issuer.IssueCard(MockedFirstName, MockedLastName));

            // Assert
            Assert.Equal("Failed to issue credit card", ex.Message);
            Assert.Equal("Mock Exception", ex.InnerException!.Message);
        }

        [Fact]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_CVCGenerator_ThrowsException()
        {
            // Arrange
            SetupMocks();
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // override
            _creditCardCVCGeneratorMock.Setup(m => m.Generate(MockedCardNumber)).Throws(new Exception("Mock Exception"));

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => issuer.IssueCard(MockedFirstName, MockedLastName));

            // Assert
            Assert.Equal("Failed to issue credit card", ex.Message);
            Assert.Equal("Mock Exception", ex.InnerException!.Message);
        }

        [Theory]
        [InlineData("First", "", "Value cannot be null. (Parameter 'LastName')")]
        [InlineData("First", null, "Value cannot be null. (Parameter 'LastName')")]
        [InlineData("First", "X", "LastName (Parameter 'Provided string exceeds expected length 1..20')")]
        [InlineData("First", "0123456789012345678901", "LastName (Parameter 'Provided string exceeds expected length 1..20')")]
        [InlineData("", "Last", "Value cannot be null. (Parameter 'FirstName')")]
        [InlineData(null, "Last", "Value cannot be null. (Parameter 'FirstName')")]
        [InlineData("X", "Last", "FirstName (Parameter 'Provided string exceeds expected length 1..20')")]
        [InlineData("0123456789012345678901", "Last", "FirstName (Parameter 'Provided string exceeds expected length 1..20')")]
        public void CreditCardIssuerRisksMitigatedTests_IssueCard_FirstLastName_ThrowsException(string firstName, string lastName, string expectedException)
        {
            // Arrange
            SetupMocks();
            var issuer = new CreditCardIssuerRisksMitigated(_timeProvider, _creditCardNumberGenerator, _creditCardCVCGenerator);

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => issuer.IssueCard(firstName, lastName));

            // Assert
            Assert.Equal(expectedException, ex.Message);
        }
    }
}