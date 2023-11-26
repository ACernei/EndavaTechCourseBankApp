using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateWallet;

public class UpdateWalletCommand : IRequest<CommandStatus>
{
    public Guid Id { get; set; }
    public string WalletType { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
}
