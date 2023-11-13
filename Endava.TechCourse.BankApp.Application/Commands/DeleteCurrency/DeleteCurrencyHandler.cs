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
            .AsNoTracking()
            .Include(currency => currency.Wallets)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (currency == null)
            return CommandStatus.Failed("Valuta nu exista.");


        context.Wallets.RemoveRange(currency.Wallets);
        context.Currencies.Remove(currency);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
