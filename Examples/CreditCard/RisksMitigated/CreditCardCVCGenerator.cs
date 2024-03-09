namespace Examples.CreditCard.RisksMitigated
{
    public class CreditCardCVCGenerator : ICreditCardCVCGenerator
    {
        private readonly IDatabase _database;

        public CreditCardCVCGenerator(IDatabase database)
        {
            _database = database;
        }

        public string Generate(string cardNumber)
        {
            // ✅ [TRANSIENT,UNEXPECTED]
            // - Input values are validated and tested
            if (string.IsNullOrEmpty(cardNumber)) throw new ArgumentNullException(nameof(cardNumber));
            if (!cardNumber.All(char.IsDigit)) throw new ArgumentException(nameof(cardNumber), "Provided string has non-numeric characters");
            if (cardNumber.Length != 16) throw new ArgumentException(nameof(cardNumber), "Provided string doesn't match expected length 16 ");

            var randomCombination = _database.GetCVCKeys(); // ✅ [TRANSIENT,UNEXPECTED], For actual CVC this could be a pair of DES (Data Encryption Standard) keys
            var cardNumberHash = (uint)cardNumber.GetHashCode();
            var combined = randomCombination ^ cardNumberHash;

            return combined.ToString()[0..3];

            // ✅  - The results are validated with tests
        }
    }
}
