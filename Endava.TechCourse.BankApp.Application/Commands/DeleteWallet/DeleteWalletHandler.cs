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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (wallet == null)
            return CommandStatus.Failed("Portofelul nu exista.");

        context.Wallets.Remove(wallet);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
