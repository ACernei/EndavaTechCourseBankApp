namespace Endava.TechCourse.BankApp.Shared;

public class WalletTypeDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal TransactionFee { get; set; }
    public bool CanBeRemoved { get; set; } = true;
}
