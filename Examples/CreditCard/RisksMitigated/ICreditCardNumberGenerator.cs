namespace Examples.CreditCard.RisksMitigated
{
    public interface ICreditCardNumberGenerator
    {
        /// <summary>
        /// Generates a credit card number
        /// </summary>
        /// <returns>Generated number</returns>
        public string Generate();
    }
}
