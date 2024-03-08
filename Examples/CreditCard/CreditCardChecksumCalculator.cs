
namespace Examples.CreditCard
{
    /// <summary>
    /// https://www.creditcardvalidator.org/articles/luhn-algorithm
    /// </summary>
    public class CreditCardChecksumCalculator : ICreditCardChecksumCalculator
    {
        public int Calculate(string cardNumber)
        {
            // ✅ [TRANSIENT,UNEXPECTED]
            // - Input values are validated and tested
            if ( string.IsNullOrEmpty(cardNumber)) throw new ArgumentNullException(nameof(cardNumber));
            if ( !cardNumber.All(char.IsDigit)) throw new ArgumentException(nameof(cardNumber), "Provided string has non-numeric characters");
            if ( cardNumber.Length < 1 && cardNumber.Length > 20 ) throw new ArgumentException(nameof(cardNumber), "Provided string exceeds expected length 1..20 ");

            // Reverse the card number
            char[] cardArray = cardNumber.ToCharArray();
            Array.Reverse(cardArray);
            string reversedCardNumber = new string(cardArray);

            int sum = 0;

            for (int i = 0; i < reversedCardNumber.Length; i++)
            {
                int digit = int.Parse(reversedCardNumber[i].ToString());    // ✅ [TRANSIENT,UNEXPECTED] - This can be not an integer - validated with a test and input arguments

                // Double every first digit
                if (i % 2 == 0)
                {
                    digit *= 2;

                    // If doubling results in a number greater than 9, subtract 9
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
            }

            // Calculate the checksum needed to make the sum a multiple of 10
            int checksum = (sum * 9) % 10;

            return checksum;

            // ✅ [TRANSIENT,UNEXPECTED] The results need to be validated, to make sure Luhn algorithm generation is correct - validated with a test
        }
    }
}
