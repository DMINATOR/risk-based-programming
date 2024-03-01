using Examples.CreditCard;

namespace ExampleTests
{
    public class CreditCardIssuerTests
    {
        [Fact]
        public void IssueCard_Default_Success()
        {
            // Arrange
            var firstName = "First";
            var lastName = "Last";
            var issuer = new CreditCardIssuer();

            // Act
            var card = issuer.IssueCard(firstName, lastName);

            // Assert
            Assert.Equal(firstName, card.FirstName);
            Assert.Equal(lastName, card.LastName);
        }
    }
}