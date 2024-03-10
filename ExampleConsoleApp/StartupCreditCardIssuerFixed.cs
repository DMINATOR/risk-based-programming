using Examples.CreditCard;
using Examples.CreditCard.RisksMitigated;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

public class StartupCardIssuerFixed
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var provider = services.BuildServiceProvider();
        
        IssueCards(provider);
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IDatabase, DatabaseMock>()
            .AddSingleton<ICreditCardNumberGenerator, CreditCardNumberGenerator>()
            .AddSingleton<ICreditCardChecksumCalculator, CreditCardChecksumCalculator>()
            .AddSingleton<ICreditCardCVCGenerator, CreditCardCVCGenerator>();
    }

    public static CreditCard IssueCard(ServiceProvider provider)
    {
        var issuer = new CreditCardIssuerRisksMitigated(
          provider.GetService<TimeProvider>(),
          provider.GetService<ICreditCardNumberGenerator>(),
          provider.GetService<ICreditCardCVCGenerator>());

        var card = issuer.IssueCard("name1", "name2");

        return card;
    }

    private static void IssueCards(ServiceProvider provider)
    {
        while (true)
        {
            var card = IssueCard(provider);

            Console.WriteLine(@$"Card issued (fixed):
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
                Console.WriteLine("ESC - to exit");
            }
        }
    }
}