using System.Diagnostics.CodeAnalysis;

namespace Examples.CreditCard.Original
{
    #region Original code

    public class CreditCardIssuer : ICreditCardIssuer
    {
        private const int ValidityYears = 5;

        public CreditCard IssueCard(DateTime date, string FirstName, string LastName)
        {
            // Calculate validity date
            var validity = new DateTime(date.Year + ValidityYears, date.Month, date.Day); // ⚠ Defective code
            var cardNumber = GenerateNumber();

            var cardDetails = new CreditCard()
            {
                FirstName = FirstName,
                LastName = LastName,

                ValidMonth = validity.Month,
                ValidYear = validity.Year,

                Number = cardNumber,

                CVC = GenerateCVC(cardNumber),
            };

            return cardDetails;
        }

        // Not a valid CVC generation algorithm
        private string GenerateCVC(string cardNumber)
        {
            var randomCombination = Random.Shared.Next(999); // For actual CVC this could be a pair of DES (Data Encryption Standard) keys
            var cardNumberHash = cardNumber.GetHashCode();
            var combined = randomCombination ^ cardNumberHash;

            return combined.ToString()[0..3];
        }

        // Calculates card number with Luhn algorithm
        public static int CalculateChecksum(string cardNumber)
        {
            // Reverse the card number
            char[] cardArray = cardNumber.ToCharArray();
            Array.Reverse(cardArray);
            string reversedCardNumber = new string(cardArray);

            int sum = 0;

            for (int i = 0; i < reversedCardNumber.Length; i++)
            {
                int digit = int.Parse(reversedCardNumber[i].ToString());

                // Double every second digit
                if (i % 2 == 1)
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
            int checksum = sum * 9 % 10;

            return checksum;
        }

        /// <summary>
        /// Generates a credit card number
        /// 
        /// https://en.wikipedia.org/wiki/ISO/IEC_7812
        /// </summary>
        /// <returns></returns>
        private string GenerateNumber()
        {
            var industryIdentifier = "5"; // [0] Banking
            var issuerIdentificationNumber = $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7]; // [1..7]
            var personalAccountNumber = $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7]; // [8..14]
            var checkSum = CalculateChecksum($"{industryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}"); // [15]

            return $"{industryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}{checkSum}";
        }
    }

    #endregion

    #region Risk analysis

    /// <summary>
    /// This class shows example of CreditCardIssuer with risks analyzed and categorized
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreditCardIssuerRiskAnalyzed : ICreditCardIssuer
    {
        // ❗ [UNDETECTED]
        // - The constant value is fixed, but can be changed in the future or set to an invalid value, exposing a potential bug that was previously hidden
        private const int ValidityYears = 5;

        public CreditCard IssueCard(DateTime date, string FirstName, string LastName)
        {
            // ❗ [TRANSIENT,SECURITY,UNEXPECTED]
            // - Input values are not validated, consider:
            // date - The DateTime is passed as is, potentially may be a local user time zone. It's better to use UTC instead and generate on server side
            //      - Range is not validated, consider it can be passed as a value in the future, making the expiration date too far in the future
            //      - Range can be bassed in the past, by mistake. Consider it will produce a card with expired expiration date
            // FirstName, LastName - These are not validated:
            //      - The value can be empty/null/invalid symbols/too long

            // ❗ [TRANSIENT,APPCRASH]
            // - The DateTime is prone to LeapYear deffect which happens on February 29th
            var validity = new DateTime(date.Year + ValidityYears, date.Month, date.Day);
            var cardNumber = GenerateNumber();

            var cardDetails = new CreditCard()
            {
                FirstName = FirstName,
                LastName = LastName,

                ValidMonth = validity.Month,
                ValidYear = validity.Year,

                Number = cardNumber,

                CVC = GenerateCVC(cardNumber),
            };

            return cardDetails;
        }

        // Not a valid CVC generation algorithm
        private string GenerateCVC(string cardNumber)
        {
            // ❗ [TRANSIENT,UNEXPECTED]
            // - Input values are not validated, consider:
            // cardNumber - can be empty/null/too small

            var randomCombination = Random.Shared.Next(999); // For actual CVC this could be a pair of DES (Data Encryption Standard) keys
            var cardNumberHash = cardNumber.GetHashCode();
            var combined = randomCombination ^ cardNumberHash;

            return combined.ToString()[0..3];

            // ❗ - The results need to be validated, to make sure the CVC generation is correct
        }

        // Calculates card number with Luhn algorithm
        public static int CalculateChecksum(string cardNumber)
        {
            // ❗ [TRANSIENT,UNEXPECTED]
            // - Input values are not validated, consider:
            // cardNumber
            //  - can be empty/null/too small
            //  - can be non numeric symbols

            // Reverse the card number
            char[] cardArray = cardNumber.ToCharArray();
            Array.Reverse(cardArray);
            string reversedCardNumber = new string(cardArray);

            int sum = 0;

            for (int i = 0; i < reversedCardNumber.Length; i++)
            {
                int digit = int.Parse(reversedCardNumber[i].ToString());  // ❗ [TRANSIENT,UNEXPECTED] - This can be not an integer, and fail

                // Double every second digit
                if (i % 2 == 1)
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
            int checksum = sum * 9 % 10;

            return checksum;

            // ❗ [TRANSIENT,UNEXPECTED] The results need to be validated, to make sure Luhn algorithm generation is correct
        }

        /// <summary>
        /// Generates a credit card number
        /// 
        /// https://en.wikipedia.org/wiki/ISO/IEC_7812
        /// </summary>
        /// <returns></returns>
        private string GenerateNumber()
        {
            var industryIdentifier = "5"; // [0] Banking
            var issuerIdentificationNumber = $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7]; // [1..7]
            var personalAccountNumber = $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7]; // [8..14]
            var checkSum = CalculateChecksum($"{industryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}"); // [15]

            return $"{industryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}{checkSum}";

            // ❗ - The results need to be validated, to make sure, different parts generate the correct response as expected
        }
    }

    #endregion
}
