using Examples.CreditCard.RisksMitigated;

namespace ExampleTests
{
    public class CreditCardChecksumCalculatorTests
    {
        [Theory]
        [InlineData("1", 8)] // Minimum length
        [InlineData("853", 2)]
        [InlineData("424242424242424",2)]
        [InlineData("7992739871", 3)]
        public void CreditCardChecksumCalculator_Calculate_Default_Success(string number, int expectedChecksum)
        {
            // Arrange
            var calculator = new CreditCardChecksumCalculator();

            // Act
            var actualChecksum = calculator.Calculate(number);

            // Assert
            Assert.Equal(expectedChecksum, actualChecksum);
        }

        [Theory]
        [InlineData("", "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData(null, "Value cannot be null. (Parameter 'cardNumber')")]
        [InlineData("abc", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("123 123", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData(" 123 ", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("9210X", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        [InlineData("-12", "cardNumber (Parameter 'Provided string has non-numeric characters')")]
        public void CreditCardChecksumCalculator_Calculate_InvalidNumber_ThrowsException(string number, string expectedMessage)
        {
            // Arrange
            var calculator = new CreditCardChecksumCalculator();

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => 
                calculator.Calculate(number)
            );

            // Assert
            Assert.Equal(expectedMessage, ex.Message);
        }
    }
}