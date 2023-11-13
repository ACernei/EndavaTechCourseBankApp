using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.AddCurrency;

public class AddCurrencyHandler : IRequestHandler<AddCurrencyCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public AddCurrencyHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
    {
        if (await context.Currencies.AnyAsync(x => x.Name == request.Name))
            return CommandStatus.Failed("O valuta cu aceasta denumire deja exista.");

        if (await context.Currencies.AnyAsync(x => x.Code == request.Code))
            return CommandStatus.Failed("O valuta cu acest cod deja exista.");

        var currency = new Currency
        {
            Name = request.Name,
            Code = request.Code,
            ChangeRate = request.ChangeRate
        };

        await context.Currencies.AddAsync(currency, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
