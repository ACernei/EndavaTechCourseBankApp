using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.SetMainWallet;

public class SetMainWalletHandler : IRequestHandler<SetMainWalletCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<User> userManager;

    public SetMainWalletHandler(ApplicationDbContext context, UserManager<User> userManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(userManager);

        this.context = context;
        this.userManager = userManager;
    }

    public async Task<CommandStatus> Handle(SetMainWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user is null) return CommandStatus.Failed("Utilizatorul nu exista");

        var wallet = await context.Wallets
            .FirstOrDefaultAsync(x => x.Id == request.WalletId, cancellationToken);

        if (wallet is null) return CommandStatus.Failed("Portofelul nu exista");

        user.MainWalletId = wallet.Id;

        await context.SaveChangesAsync(cancellationToken);

        return new CommandStatus();
    }
}
