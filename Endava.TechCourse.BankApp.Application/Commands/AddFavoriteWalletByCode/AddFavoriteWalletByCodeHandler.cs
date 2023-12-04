using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.AddFavoriteWalletByCode;

public class AddFavoriteWalletByCodeHandler : IRequestHandler<AddFavoriteWalletByCodeCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public AddFavoriteWalletByCodeHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(AddFavoriteWalletByCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(x => x.FavoriteWallets)
            .Include(x => x.Wallets)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
            return CommandStatus.Failed($"Userul {request.UserId} nu a fost gasit");

        var favoriteWallet = await context.Wallets
            .FirstOrDefaultAsync(x => x.Code == request.WalletCode, cancellationToken);
        if (favoriteWallet is null)
            return CommandStatus.Failed("Portofel nu exista.");

        if (!user.FavoriteWallets.Contains(favoriteWallet))
            user.FavoriteWallets.Add(favoriteWallet);

        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
