
namespace Examples.CreditCard.RisksMitigated
{
    public class CreditCardNumberGenerationFailedException : Exception
    {
        public CreditCardNumberGenerationFailedException(Exception baseException) : base("Failed to generate card number", baseException)
        {

        }
    }
}
