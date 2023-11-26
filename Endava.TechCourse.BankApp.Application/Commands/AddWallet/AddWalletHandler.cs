using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddWallet;

public class AddWalletHandler : IRequestHandler<AddWalletCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public AddWalletHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = new Wallet
        {
            WalletType = request.WalletType,
            Amount = request.Amount,
            CurrencyId = request.CurrencyId,
            UserId = request.UserId
        };

        await context.Wallets.AddAsync(wallet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
