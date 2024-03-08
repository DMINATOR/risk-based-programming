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
        private string MockedPersonalAccountNumber = "1234567";
        private string MockedIssuerIdentificationNumber = "1234567";

        public CreditCardNumberGeneratorTests()
        {
            _checksumCalculatorMock = new Mock<ICreditCardChecksumCalculator>();
            _checksumCalculator = _checksumCalculatorMock.Object;

            _databaseMock = new Mock<IDatabase>();
            _database = _databaseMock.Object;
        }

        private string SetupMocks()
        {
            var creditCardNumberPart = $"{CreditCardNumberGenerator.IndustryIdentifier}{MockedIssuerIdentificationNumber}{MockedPersonalAccountNumber}";

            _databaseMock.Setup(m => m.GetPersonalAccountNumber()).Returns(MockedPersonalAccountNumber);
            _databaseMock.Setup(m => m.GetIssuerIdentificationNumber()).Returns(MockedIssuerIdentificationNumber);
            _checksumCalculatorMock.Setup(m => m.Calculate(creditCardNumberPart)).Returns(2);

            return creditCardNumberPart;
        }

        [Fact]
        public void CreditCardNumberGeneratorTests_Generate_Default_Success()
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);
            var creditCardNumberPart = SetupMocks();

            // Act
            var number = generator.Generate();

            // Assert
            _databaseMock.VerifyAll();
            _checksumCalculatorMock.VerifyAll();

            Assert.Equal(16, number.Length);
            Assert.Equal($"{creditCardNumberPart}{2}", number);
            Assert.All(number, x => char.IsDigit(x));
        }

        [Fact]
        public void CreditCardNumberGeneratorTests_Generate_GetPersonalAccountNumber_ThrowsException()
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);
            var creditCardNumberPart = SetupMocks();

            // override
            _databaseMock.Setup(m => m.GetPersonalAccountNumber()).Throws(new Exception("Mock Exception"));

            // Act
            var ex = Assert.ThrowsAny<Exception>(generator.Generate);

            // Assert
            Assert.Equal("Failed to generate card number", ex.Message);
            Assert.Equal("Mock Exception", ex.InnerException!.Message);
        }

        [Fact]
        public void CreditCardNumberGeneratorTests_Generate_GetIssuerIdentificationNumber_ThrowsException()
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);
            var creditCardNumberPart = SetupMocks();

            // override
            _databaseMock.Setup(m => m.GetIssuerIdentificationNumber()).Throws(new Exception("Mock Exception"));

            // Act
            var ex = Assert.ThrowsAny<Exception>(generator.Generate);

            // Assert
            Assert.Equal("Failed to generate card number", ex.Message);
            Assert.Equal("Mock Exception", ex.InnerException!.Message);
        }

        [Fact]
        public void CreditCardNumberGeneratorTests_Generate_ChecksumCalculator_ThrowsException()
        {
            // Arrange
            var generator = new CreditCardNumberGenerator(_checksumCalculator, _database);
            var creditCardNumberPart = SetupMocks();

            // override
            _checksumCalculatorMock.Setup(m => m.Calculate(creditCardNumberPart)).Throws(new Exception("Mock Exception"));

            // Act
            var ex = Assert.ThrowsAny<Exception>(generator.Generate);

            // Assert
            Assert.Equal("Failed to generate card number", ex.Message);
            Assert.Equal("Mock Exception", ex.InnerException!.Message);
        }
    }
}