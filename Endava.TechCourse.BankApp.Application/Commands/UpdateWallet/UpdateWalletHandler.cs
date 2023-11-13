using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateWallet;

public class UpdateWalletHandler : IRequestHandler<UpdateWalletCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public UpdateWalletHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await context.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (wallet == null)
            return CommandStatus.Failed("Portofelul nu exista.");

        wallet.Type = request.Type;
        wallet.Amount = request.Amount;
        wallet.CurrencyId = request.CurrencyId;

        context.Wallets.Update(wallet);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
