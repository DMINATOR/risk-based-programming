using Examples.CreditCard;

namespace ExampleTests
{
    public class CreditCardIssuerTests
    {
        // Simple test, that returns a 100% code coverage of all lines, but is it sufficient ?
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