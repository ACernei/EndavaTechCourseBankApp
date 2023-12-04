using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.CreateTransactionByCode;

public class CreateTransactionByCodeCommand : IRequest<CommandStatus>
{
    public Guid InitiatorUserId { get; set; }
    public Guid CurrencyId { get; set; }
    public string TransactionType { get; set; }
    public Guid SourceWalletId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TargetWalletCode { get; set; }
}
