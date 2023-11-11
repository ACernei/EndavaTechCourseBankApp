namespace Endava.TechCourse.BankApp.Shared;

public class WalletDto
{
    public string Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public CurrencyDto Currency { get; set; }
}
