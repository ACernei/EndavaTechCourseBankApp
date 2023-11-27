using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateWalletType;

public class UpdateWalletTypeHandler : IRequestHandler<UpdateWalletTypeCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;

    public UpdateWalletTypeHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<CommandStatus> Handle(UpdateWalletTypeCommand request, CancellationToken cancellationToken)
    {
        var walletType = await context.WalletTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (walletType is null)
            return CommandStatus.Failed("Tipul nu exista.");

        if (await context.WalletTypes.AnyAsync(x => x.Name == request.Name && x.Id != request.Id, cancellationToken))
            return CommandStatus.Failed("Tip cu aceasta denumire deja exista.");

        walletType.Name = request.Name;
        walletType.TransactionFee = request.TransactionFee;

        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
