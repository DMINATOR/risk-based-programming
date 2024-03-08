
namespace Examples.CreditCard.RisksMitigated
{
    public interface ICreditCardCVCGenerator
    {
        /// <summary>
        /// Generates CVC from credit card number
        /// </summary>
        public string Generate(string number);
    }
}
