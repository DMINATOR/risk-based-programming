
namespace Examples.CreditCard.RisksMitigated
{
    public class CreditCardIssuingFailedException : Exception
    {
        public CreditCardIssuingFailedException(Exception baseException) : base("Failed to issue credit card", baseException)
        {

        }
    }
}
