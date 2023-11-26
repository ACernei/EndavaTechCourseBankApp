using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.DeleteWallet;

public class DeleteWalletHandler : IRequestHandler<DeleteWalletCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public DeleteWalletHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await context.Wallets
            .Include(x => x.InitiatedTransactions)
            .Include(x => x.ReceivedTransactions)
            .FirstOrDefaultAsync(wallet => wallet.Id == request.Id, cancellationToken);

        if (wallet is null)
            return CommandStatus.Failed("Portofelul nu exista.");

        context.RemoveRange(wallet.InitiatedTransactions);
        context.RemoveRange(wallet.ReceivedTransactions);
        context.Wallets.Remove(wallet);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
