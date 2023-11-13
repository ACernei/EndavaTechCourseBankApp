using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.UpdateCurrency;

public class UpdateCurrencyCommand : IRequest<CommandStatus>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal ChangeRate { get; set; }
}