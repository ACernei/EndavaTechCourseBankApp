namespace Endava.TechCourse.BankApp.Shared;

public class TransactionDto
{
    public WalletDto Source { get; set; }
    public WalletDto Target { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}
