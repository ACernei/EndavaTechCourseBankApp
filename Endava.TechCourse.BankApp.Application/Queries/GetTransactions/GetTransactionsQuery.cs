using Endava.TechCourse.BankApp.Domain.Models;
using MediatR;

namespace Endava.TechCourse.BankApp.Application.Queries.GetTransactions;

public class GetTransactionsQuery : IRequest<List<Transaction>>
{
    public Guid InitiatorUserId { get; set; }
}
