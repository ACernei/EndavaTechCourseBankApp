using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public CreateTransactionHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionCurrency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.CurrencyId, cancellationToken);
        var source = await context.Wallets
            .Include(wallet => wallet.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.SourceWalletId && x.UserId == request.InitiatorUserId,
                cancellationToken);
        var target = await context.Wallets
            .Include(wallet => wallet.Currency)
            .FirstOrDefaultAsync(x => x.Id == request.TargetWalletId, cancellationToken);

        if (transactionCurrency is null)
            return CommandStatus.Failed("Valuta nu exista.");
        if (source is null)
            return CommandStatus.Failed("Portofelul nu exista.");
        if (target is null)
            return CommandStatus.Failed("Portofelul nu exista.");

        var givenAmount = request.Amount * transactionCurrency.ChangeRate / source.Currency.ChangeRate;
        var receivedAmount = request.Amount * transactionCurrency.ChangeRate / target.Currency.ChangeRate;
        var commission = 0;
        source.Amount -= givenAmount + commission;
        target.Amount += receivedAmount;

        var transaction = new Transaction
        {
            TransactionType = request.TransactionType,
            Source = source,
            Target = target,
            Amount = request.Amount,
            Currency = transactionCurrency,
            Description = request.Description
        };
        await context.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
