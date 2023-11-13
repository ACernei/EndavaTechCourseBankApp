using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Queries.GetWalletById;

public class GetWalletByIdHandler : IRequestHandler<GetWalletByIdQuery, Wallet>
{
    private readonly ApplicationDbContext context;

    public GetWalletByIdHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<Wallet> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await context.Wallets
            .Include(x => x.Currency)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return wallet;
    }
}
