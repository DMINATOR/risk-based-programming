using Examples.CreditCard.RisksMitigated;
using Moq;

namespace ExampleTests.Unit
{
    public class CreditCardNumberGeneratorTests
    {
        // Database
        private Mock<IDatabase> _databaseMock;
        private IDatabase _database;

        // Checksum
        private Mock<ICreditCardChecksumCalculator> _checksumCalculatorMock;
        private ICreditCardChecksumCalculator _checksumCalculator;

        // Values
        private string MockedPersonalAccountNumber = "01234567";
        private string MockedIssuerIdentificationNumber = "01234567";

        public CreditCardNumberGeneratorTests()
        {
            _checksumCalculatorMock = new Mock<ICreditCardChecksumCalculator>();
            _checksumCalculator = _checksumCalculatorMock.Object;

            _databaseMock = new Mock<IDatabase>();
            _database = _databaseMock.Object;
        }

        [Fact]
        public void CreditCardNumberGeneratorTests_Default_Success()
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);

            var creditCardNumberPart = $"{CreditCardNumberGenerator.IndustryIdentifier}{MockedIssuerIdentificationNumber}{MockedPersonalAccountNumber}";
            _databaseMock.Setup(m => m.GetPersonalAccountNumber()).Returns(MockedPersonalAccountNumber);
            _databaseMock.Setup(m => m.GetIssuerIdentificationNumber()).Returns(MockedIssuerIdentificationNumber);
            _checksumCalculatorMock.Setup(m => m.Calculate(creditCardNumberPart)).Returns(2);

            // Act
            var number = generator.Generate();

            // Assert
            _databaseMock.VerifyAll();
            _checksumCalculatorMock.VerifyAll();

            Assert.Equal(16, number.Length);
            Assert.Equal($"{creditCardNumberPart}{2}", number);
        }

        [Theory]
        [InlineData("", "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData(null, "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData("abc", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("123 123", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData(" 123 ", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("9210X", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("-12", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        public void CreditCardNumberGeneratorTests_InvalidNumber_ThrowsException(string number, string expectedMessage)
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);

            // Act
            var ex = Assert.ThrowsAny<Exception>(generator.Generate);

            // Assert
            Assert.Equal(expectedMessage, ex.Message);
        }
    }
}