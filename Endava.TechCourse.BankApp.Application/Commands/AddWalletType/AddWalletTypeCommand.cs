using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddCurrency;

public class AddWalletTypeCommand : IRequest<CommandStatus>
{
    public string Name { get; set; }
    public decimal TransactionFee { get; set; }
}