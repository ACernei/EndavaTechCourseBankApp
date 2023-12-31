using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddWallet;

public class AddWalletCommand : IRequest<CommandStatus>
{
    public Guid WalletTypeId { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public Guid UserId { get; set; }
}
