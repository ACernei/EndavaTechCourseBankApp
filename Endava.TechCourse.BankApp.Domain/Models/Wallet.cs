using Endava.TechCourse.BankApp.Domain.Common;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class Wallet : BaseEntity
{
    public string Code { get; set; }
    public Guid WalletTypeId { get; set; }

    public WalletType WalletType { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Transaction> InitiatedTransactions { get; set; }
    public ICollection<Transaction> ReceivedTransactions { get; set; }

    public bool IsMain()
    {
        return User.MainWalletId == Id;
    }
}