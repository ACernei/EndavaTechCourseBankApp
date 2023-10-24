using Endava.TechCourse.BankApp.Domain.Common;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class Report : BaseEntity
{
    public string Title { get; set; }
    public List<Transaction> Transactions { get; set; }
}
