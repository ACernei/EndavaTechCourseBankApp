using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.AddWalletType;

public class AddWalletTypeHandler : IRequestHandler<AddWalletTypeCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public AddWalletTypeHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(AddWalletTypeCommand request, CancellationToken cancellationToken)
    {
        if (await context.WalletTypes.AnyAsync(x => x.Name == request.Name, cancellationToken))
            return CommandStatus.Failed("O valuta cu aceasta denumire deja exista.");

        var walletType = new WalletType
        {
            Name = request.Name,
            TransactionFee = request.TransactionFee
        };

        await context.WalletTypes.AddAsync(walletType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
