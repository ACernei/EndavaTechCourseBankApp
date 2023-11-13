using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddCurrency;

public class AddCurrencyCommand : IRequest<CommandStatus>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal ChangeRate { get; set; }
}
