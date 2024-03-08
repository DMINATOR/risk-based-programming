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
            try
            {
                var issuerIdentificationNumber = _database.GetIssuerIdentificationNumber(); // [1..7]
                var personalAccountNumber = _database.GetPersonalAccountNumber(); // [8..14]
                var checkSum = _checksumCalculator.Calculate($"{IndustryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}"); // [15]

                return $"{IndustryIdentifier}{issuerIdentificationNumber}{personalAccountNumber}{checkSum}";
            }
            catch (DatabaseException ex)
            {
                // ✅ [TRANSIENT,UNEXPECTED] Assuming these will be issues by a database, the results can return invalid results or fail at random times
                // All database exceptions are handled differently here
                throw new CreditCardNumberGenerationFailedException(ex);
            }
            catch (Exception ex)
            {
                // All other exceptions
                throw new CreditCardNumberGenerationFailedException(ex);
            }

            // ✅ - The results need to be validated, to make sure, different parts generate the correct response as expected - these are tested
        }
    }
}
