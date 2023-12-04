namespace Endava.TechCourse.BankApp.Shared;

public class WalletDto
{
    public string Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreationDate { get; set; }
    public string WalletTypeId { get; set; }
    public string CurrencyId { get; set; }
    public bool IsMain { get; set; }
    public decimal TransactionFee { get; set; }
}
