using Examples.CreditCard.Original;
using System.Text;

public class StartupCardIssuer
{
    private static readonly CreditCardIssuer _issuer = new CreditCardIssuer();

    public static void Main(string[] args)
    {
        IssueCards();
    }

    private static void IssueCards()
    {
        var now = DateTime.Now;

        while (true)
        {
            var card = _issuer.IssueCard(now, "name1", "name2");

            Console.WriteLine(@$"Card issued:
First Name: {card.FirstName}
Last Name: {card.LastName}
Number: {card.Number}
Valid: {card.ValidMonth}/{card.ValidYear}
CVC: {card.CVC}
");
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Escape)
            {
                return;
            }
            else
            {
                Console.WriteLine("ESC - to exit, [L] - for leap year");

                if (key.Key == ConsoleKey.L)
                {
                    now = new DateTime(2024, 02, 29); // ⚠ Defect - Leap year happens !
                }
            }
        }
    }
}