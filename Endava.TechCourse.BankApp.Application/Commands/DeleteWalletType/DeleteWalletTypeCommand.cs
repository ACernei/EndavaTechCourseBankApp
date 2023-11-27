using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;

public class DeleteWalletTypeCommand : IRequest<CommandStatus>
{
    public Guid Id { get; set; }
}
