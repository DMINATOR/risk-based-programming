using Examples.CreditCard;

namespace ExampleTests
{
    public class CreditCardIssuerRisksMitigatedTests
    {
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