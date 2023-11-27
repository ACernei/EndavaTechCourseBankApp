using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateWalletType;

public class UpdateWalletTypeCommand : IRequest<CommandStatus>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal TransactionFee { get; set; }
}