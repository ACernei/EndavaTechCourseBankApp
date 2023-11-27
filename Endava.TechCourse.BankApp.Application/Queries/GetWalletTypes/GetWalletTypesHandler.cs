using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Queries.GetWalletTypes;

public class GetWalletTypesHandler : IRequestHandler<GetWalletTypesQuery, List<WalletType>>
{
    private readonly ApplicationDbContext context;

    public GetWalletTypesHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<List<WalletType>> Handle(GetWalletTypesQuery request, CancellationToken cancellationToken)
    {
        var walletTypes = await context.WalletTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return walletTypes;
    }
}
