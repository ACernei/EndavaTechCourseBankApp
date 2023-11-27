using Endava.TechCourse.BankApp.Domain.Common;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class WalletType : BaseEntity
{
    public string Name { get; set; }
    public decimal TransactionFee { get; set; }
    public List<Wallet> Wallets { get; set; } = new();
}