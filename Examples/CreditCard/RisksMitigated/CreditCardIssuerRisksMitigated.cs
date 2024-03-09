using System.Diagnostics.CodeAnalysis;

namespace Examples.CreditCard.RisksMitigated
{
    /// <summary>
    /// This is a new version of class, redesigned to reduce potential risks.
    /// 
    /// The goals are:
    /// - Make each section testable for each type of potential risk and prove that they are mitigated
    /// - Break each peace apart to make it easier to test
    /// - Provide different conditions and scenarios
    /// 
    /// </summary>
    public class CreditCardIssuerRisksMitigated : ICreditCardIssuerRisksMitigated
    {
        // Dependencies
        private readonly TimeProvider _timeProvider;
        private readonly ICreditCardNumberGenerator _numberGenerator;
        private readonly ICreditCardCVCGenerator _cvcGenerator;

        // Settings
        private readonly int _validityYears;

        public CreditCardIssuerRisksMitigated(
            TimeProvider timeProvider,
            ICreditCardNumberGenerator numberGenerator,
            ICreditCardCVCGenerator cvcGenerator,
            int validityYears = 5)
        {
            // ✅ [TRANSIENT,UNEXPECTED]
            // - Input values are validated and tested
            if (timeProvider == null) throw new ArgumentNullException(nameof(timeProvider));
            if (numberGenerator == null) throw new ArgumentNullException(nameof(numberGenerator));
            if (cvcGenerator == null) throw new ArgumentNullException(nameof(cvcGenerator));
            if (validityYears < 0 || validityYears > 99) throw new ArgumentException(nameof(validityYears), "Provided validity years is outside expected range 0..99");

            _validityYears = validityYears;
            _timeProvider = timeProvider;
            _numberGenerator = numberGenerator;
            _cvcGenerator = cvcGenerator;
        }

        public CreditCard IssueCard(string FirstName, string LastName)
        {
            // Validate inputs

            // Calculate validity date
            var validity = _timeProvider.GetUtcNow().AddYears(_validityYears); // Not exposed to leap year bug
            var cardNumber = _numberGenerator.Generate();

            var cardDetails = new CreditCard()
            {
                FirstName = FirstName,
                LastName = LastName,

                ValidMonth = validity.Month,
                ValidYear = validity.Year,

                Number = cardNumber,

                CVC = _cvcGenerator.Generate(cardNumber),
            };

            return cardDetails;
        }
    }
}
