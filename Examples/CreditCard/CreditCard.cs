
namespace Examples.CreditCard
{
    /// <summary>
    /// Defines a credit card details
    /// </summary>
    public class CreditCard
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Validation date
        public int ValidMonth { get; set; }
        
        public int ValidYear { get; set; }

        // Full number
        // XXXX XXXX XXXX XXXX
        public string Number { get; set; }

        // CVC
        public string CVC { get; set; }
    }
}
