using Examples.CreditCard.RisksMitigated;
using Moq;

namespace ExampleTests.Unit
{
    public class CreditCardCVCGeneratorTests
    {
        // Database
        private Mock<IDatabase> _databaseMock;
        private IDatabase _database;

        public CreditCardCVCGeneratorTests()
        {
            _databaseMock = new Mock<IDatabase>();
            _database = _databaseMock.Object;
        }

        private void SetupMocks()
        {
            _databaseMock.Setup(m => m.GetCVCKeys()).Returns(123456);
        }

        [Theory]
        [InlineData("4242424242424242")]
        [InlineData("0000000000000000")]
        [InlineData("2222222222222222")]
        public void CreditCardCVCGenerator_Generate_Default_Success(string number)
        {
            // Arrange
            var generator = new CreditCardCVCGenerator(_database);
            SetupMocks();

            // Act
            var actualChecksum = generator.Generate(number);

            // Assert
            _databaseMock.VerifyAll();
            Assert.Equal(3, actualChecksum.Length);

            var cardNumberHash = (uint)number.GetHashCode();
            var expectedCVC = 123456 ^ cardNumberHash;
            Assert.Equal(expectedCVC.ToString()[0..3], actualChecksum);
        }

        [Theory]
        [InlineData("", "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData(null, "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData("abc", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("123 123", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData(" 123 ", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("123", "cardNumber (Parameter 'Provided string doesn't match expected length 16 ')")]
        [InlineData("42424242424242429", "cardNumber (Parameter 'Provided string doesn't match expected length 16 ')")]
        public void CreditCardChecksumCalculator_Calculate_InvalidNumber_ThrowsException(string number, string expectedMessage)
        {
            // Arrange
            var generator = new CreditCardCVCGenerator(_database);
            SetupMocks();

            // Act
            var ex = Assert.ThrowsAny<Exception>(() =>
                generator.Generate(number)
            );

            // Assert
            Assert.Equal(expectedMessage, ex.Message);
        }
    }
}