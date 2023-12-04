using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.CreateTransactionByEmail;

public class CreateTransactionByEmailHandler : IRequestHandler<CreateTransactionByEmailCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public CreateTransactionByEmailHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(CreateTransactionByEmailCommand request,
        CancellationToken cancellationToken)
    {
        var transactionCurrency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.CurrencyId, cancellationToken);
        if (transactionCurrency is null)
            return CommandStatus.Failed("Valuta nu exista.");

        var source = await context.Wallets
            .Include(wallet => wallet.Currency).Include(wallet => wallet.WalletType)
            .FirstOrDefaultAsync(x => x.Id == request.SourceWalletId && x.UserId == request.InitiatorUserId,
                cancellationToken);
        if (source is null)
            return CommandStatus.Failed("Portofelul nu exista.");

        var targetUser = await context.Users
            .Include(x => x.MainWallet)
            .ThenInclude(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Email == request.TargetEmail, cancellationToken);
        var target = targetUser?.MainWallet;
        if (targetUser is null || target is null)
            return CommandStatus.Failed("Portofelul nu exista.");

        var givenAmount = request.Amount * transactionCurrency.ChangeRate / source.Currency.ChangeRate;
        var receivedAmount = request.Amount * transactionCurrency.ChangeRate / target.Currency.ChangeRate;
        source.Amount -= givenAmount + source.WalletType.TransactionFee * givenAmount / 100;
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
