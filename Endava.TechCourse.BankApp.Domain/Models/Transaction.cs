using Endava.TechCourse.BankApp.Domain.Common;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class Transaction : BaseEntity
{
    public string TransactionType { get; set; }
    public Guid SourceId { get; set; }
    public Wallet Source { get; set; }
    public Guid TargetId { get; set; }
    public Wallet Target { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public string Description { get; set; }
}
