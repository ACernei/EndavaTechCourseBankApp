using System.Text;
using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var user = await context.Users
            .Include(x => x.Wallets)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
            return CommandStatus.Failed($"Userul {request.UserId} nu a fost gasit");

        var walletType = await context.WalletTypes
            .FirstOrDefaultAsync(x => x.Id == request.WalletTypeId, cancellationToken);
        if (walletType is null)
            return CommandStatus.Failed("Tip portofel nu exista.");

        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.CurrencyId, cancellationToken);
        if (currency is null)
            return CommandStatus.Failed("Valuta nu exista.");

        var wallet = new Wallet
        {
            Code = Create16DigitString(),
            WalletTypeId = request.WalletTypeId,
            Amount = request.Amount,
            CurrencyId = request.CurrencyId,
            UserId = request.UserId
        };

        if (user.Wallets.Count == 0)
            user.MainWalletId = wallet.Id;

        await context.Wallets.AddAsync(wallet, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }

    private string Create16DigitString()
    {
        var rng = new Random();

        var builder = new StringBuilder();
        while (builder.Length < 16) builder.Append(rng.Next(10).ToString());

        return builder.ToString();
    }
}
