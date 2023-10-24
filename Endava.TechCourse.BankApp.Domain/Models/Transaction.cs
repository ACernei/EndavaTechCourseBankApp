using Endava.TechCourse.BankApp.Domain.Common;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class Transaction : BaseEntity
{
    public Wallet Source { get; set; }
    public Wallet Target { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}
