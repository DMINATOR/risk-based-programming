namespace Examples.CreditCard.RisksMitigated
{
    public interface IDatabase
    {
        // Returns issuer identification number
        public string GetIssuerIdentificationNumber();

        // Returns personal account number
        public string GetPersonalAccountNumber();
    }
}
