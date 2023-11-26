namespace Endava.TechCourse.BankApp.Shared;

public class TransactionDto
{
    public string TransactionType { get; set; }
    public string SourceId { get; set; }
    public string TargetId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string CurrencyId { get; set; }
}
