using Examples.CreditCard.Original;
using System.Text;

public class StartupCardIssuer
{
    private static readonly CreditCardIssuer _issuer = new CreditCardIssuer();

    public static void Main(string[] args)
    {
        while(true)
        {
            var now = DateTime.Now;
            //var now = new DateTime(29,02,2024); // ⚠ Defect - Leap year happens !
            var card = _issuer.IssueCard(now, "name1", "name2");

            Console.WriteLine(@$"Card issued:
First Name: {card.FirstName}
Last Name: {card.LastName}
Number: {card.Number}
Valid: {card.ValidMonth}/{card.ValidYear}
CVC: {card.CVC}
");
            var exit = Console.ReadKey();

            if (exit.Key == ConsoleKey.Escape)
            {
                return;
            }
            else
            {
                Console.WriteLine("ESC - to exit");
            }
        }
      
    }
}