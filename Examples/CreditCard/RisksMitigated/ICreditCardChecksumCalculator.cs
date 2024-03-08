namespace Examples.CreditCard.RisksMitigated
{
    public interface ICreditCardChecksumCalculator
    {
        /// <summary>
        /// Calculates Checksum for credit card number, using Luhn algorithm
        /// </summary>
        /// <param name="number">Credit card number without checksum</param>
        /// <returns>Single digit</returns>
        public int Calculate(string number);
    }
}
