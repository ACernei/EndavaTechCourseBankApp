using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Queries.GetTransactions;

public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, List<Transaction>>
{
    private readonly ApplicationDbContext context;

    public GetTransactionsHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<List<Transaction>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await context.Transactions
            .Where(x => x.Source.UserId == request.InitiatorUserId || x.Target.UserId == request.InitiatorUserId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return transactions;
    }
}
