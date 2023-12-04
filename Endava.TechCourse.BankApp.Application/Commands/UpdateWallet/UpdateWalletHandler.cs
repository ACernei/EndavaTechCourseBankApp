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
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (wallet is null)
            return CommandStatus.Failed("Portofelul nu exista.");

        var walletType = await context.Wallets
            .FirstOrDefaultAsync(x => x.Id == request.WalletTypeId, cancellationToken);
        if (walletType is null)
            return CommandStatus.Failed("Tip portofel nu exista.");

        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.CurrencyId, cancellationToken);
        if (currency is null)
            return CommandStatus.Failed("Valuta nu exista.");

        wallet.WalletTypeId = request.WalletTypeId;
        wallet.Amount = request.Amount;
        wallet.CurrencyId = request.CurrencyId;

        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
