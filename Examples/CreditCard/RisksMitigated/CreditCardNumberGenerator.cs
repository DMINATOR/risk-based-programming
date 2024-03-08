namespace Examples.CreditCard.RisksMitigated
{
    public class CreditCardNumberGenerator : ICreditCardNumberGenerator
    {
        private readonly ICreditCardChecksumCalculator _checksumCalculator;
        private readonly IDatabase _database;

        public static readonly string IndustryIdentifier = "5";

        public CreditCardNumberGenerator(ICreditCardChecksumCalculator checksumCalculator, IDatabase database)
        {
            _checksumCalculator = checksumCalculator;
            _database = database;
        }

        public string Generate()
        {
            // ❗ [TRANSIENT,UNEXPECTED] Assuming these will be issues by a database, the results can return invalid results or fail at random times
            var issuerIdentificationNumber = _database.GetIssuerIdentificationNumber(); // [1..7]

            // ❗ [TRANSIENT,UNEXPECTED] Assuming these will be issues by a database, the results can return invalid results or fail at random times
            var personalAccountNumber = _database.GetPersonalAccountNumber(); // [8..14]

            var checkSum = _checksumCalculator.Calculate($"{IndustryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}"); // [15]

            return $"{IndustryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}{checkSum}";

            // ❗ - The results need to be validated, to make sure, different parts generate the correct response as expected
        }
    }
}
