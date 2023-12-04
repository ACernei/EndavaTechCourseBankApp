using Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.DeleteWalletType;

public class DeleteWalletTypeHandler : IRequestHandler<DeleteWalletTypeCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public DeleteWalletTypeHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(DeleteWalletTypeCommand request, CancellationToken cancellationToken)
    {
        var walletType = await context.WalletTypes
            .Include(x => x.Wallets)
            .ThenInclude(x => x.InitiatedTransactions)
            .Include(x => x.Wallets)
            .ThenInclude(x => x.ReceivedTransactions)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (walletType is null)
            return CommandStatus.Failed("Tip nu exista.");

        foreach (var wallet in walletType.Wallets.ToList())
        {
            context.RemoveRange(wallet.InitiatedTransactions);
            context.RemoveRange(wallet.ReceivedTransactions);
            context.Wallets.Remove(wallet);
        }

        context.WalletTypes.Remove(walletType);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
