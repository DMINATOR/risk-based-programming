using Examples.CreditCard;

namespace ExampleTests
{
    public class CreditCardIssuerFixedTests
    {
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