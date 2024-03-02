using System;

namespace Examples.CreditCard
{
    public class CreditCardIssuer : ICreditCardIssuer
    {
        private const int ValidityYears = 5;

        // Card number digits

        public CreditCard IssueCard(DateTime date, string FirstName, string LastName)
        {
            // Add years
            var validity = new DateTime(date.Year + ValidityYears, date.Month, date.Day); // ⚠ Defective code

            var cardDetails = new CreditCard()
            {
                FirstName = FirstName,
                LastName = LastName,

                ValidMonth = validity.Month,
                ValidYear = validity.Year,

                CVC = GenerateCVC(),

                Number = GenerateNumber()
            };

            return cardDetails;
        }

        private string GenerateCVC()
        {
            return $"{Random.Shared.Next(9)}{Random.Shared.Next(9)}{Random.Shared.Next(9)}";
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
            var issuerIdentificationNumber = $"{Random.Shared.Next(9999999)}".PadLeft(6); // [1..7]
            var personalAccountNumber = $"{Random.Shared.Next(9999999)}".PadLeft(6); // [8..14]
            var checkSum = $"{Random.Shared.Next(9)}"; // [15]

            return $"{industryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}{checkSum}";
        }
    }
}
