using Examples.CreditCard;

namespace ExampleTests
{
    public class CreditCardIssuerRisksMitigatedTests
    {
        #region Checksum tests

        [Theory]
        [InlineData("853", 2)]
        [InlineData("424242424242424",2)]
        [InlineData("7992739871", 3)]
        public void CreditCardChecksumCalculator_Default_Success(string number, int expectedChecksum)
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
        public void CreditCardChecksumCalculator_InvalidNumber_ThrowsException(string number, string expectedMessage)
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

        #endregion

        [Fact]
        public void IssueCard_Default_Success()
        {
            // Arrange
            var date = DateTime.Now;
            var firstName = "First";
            var lastName = "Last";
            var issuer = new CreditCardIssuer();

            // Act
            var card = issuer.IssueCard(date, firstName, lastName);

            // Assert
            Assert.Equal(firstName, card.FirstName);
            Assert.Equal(lastName, card.LastName);
        }
    }
}