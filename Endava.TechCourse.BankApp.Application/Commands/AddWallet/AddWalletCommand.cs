using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddWallet;

public class AddWalletCommand : IRequest<CommandStatus>
{
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
}
