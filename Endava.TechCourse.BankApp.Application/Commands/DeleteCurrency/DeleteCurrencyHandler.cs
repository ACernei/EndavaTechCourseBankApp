using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;

public class DeleteCurrencyHandler : IRequestHandler<DeleteCurrencyCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public DeleteCurrencyHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await context.Currencies
            .Include(currency => currency.Wallets)
            .ThenInclude(x => x.InitiatedTransactions)
            .Include(currency => currency.Wallets)
            .ThenInclude(x => x.ReceivedTransactions)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (currency is null)
            return CommandStatus.Failed("Valuta nu exista.");

        foreach (var wallet in currency.Wallets.ToList())
        {
            context.RemoveRange(wallet.InitiatedTransactions);
            context.RemoveRange(wallet.ReceivedTransactions);
            context.Wallets.Remove(wallet);
        }

        context.Currencies.Remove(currency);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
