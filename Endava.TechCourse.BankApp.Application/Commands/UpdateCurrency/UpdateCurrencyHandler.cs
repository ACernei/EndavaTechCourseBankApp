using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateCurrency;

public class UpdateCurrencyHandler : IRequestHandler<UpdateCurrencyCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public UpdateCurrencyHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (currency is null)
            return CommandStatus.Failed("Valuta nu exista.");

        if (await context.Currencies.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            return CommandStatus.Failed("O valuta cu aceasta denumire deja exista.");

        if (await context.Currencies.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            return CommandStatus.Failed("O valuta cu acest cod deja exista.");

        currency.Name = request.Name;
        currency.Code = request.Code;
        currency.ChangeRate = request.ChangeRate;

        context.Currencies.Update(currency);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}