using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.SetMainWallet;

public class SetMainWalletCommand : IRequest<CommandStatus>
{
    public Guid WalletId { get; set; }
    public Guid UserId { get; set; }
}
