using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Queries.GetFavoriteWallets;

public class GetFavoriteWalletsHandler : IRequestHandler<GetFavoriteWalletsQuery, List<Wallet>>
{
    private readonly ApplicationDbContext context;

    public GetFavoriteWalletsHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<List<Wallet>> Handle(GetFavoriteWalletsQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(x => x.FavoriteWallets)
            .ThenInclude(x => x.User)
            .Include(x => x.FavoriteWallets)
            .ThenInclude(x => x.WalletType)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user is null)
            return new List<Wallet>();

        return user.FavoriteWallets.ToList();
    }
}
