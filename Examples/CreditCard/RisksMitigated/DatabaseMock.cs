
namespace Examples.CreditCard.RisksMitigated
{
    /// <summary>
    /// Mock for actual database
    /// </summary>
    public class DatabaseMock : IDatabase
    {
        public string GetIssuerIdentificationNumber()
        {
            return $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7];
        }

        public string GetPersonalAccountNumber()
        {
            return $"{Random.Shared.Next(9999999)}".PadRight(7).Replace(' ', '0')[0..7];
        }
    }

    public class DatabaseException : Exception
    {

    }
}
