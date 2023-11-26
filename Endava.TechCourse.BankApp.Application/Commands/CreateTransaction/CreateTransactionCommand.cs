using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<CommandStatus>
{
    public Guid InitiatorUserId { get; set; }
    public Guid CurrencyId { get; set; }
    public string TransactionType { get; set; }
    public Guid SourceWalletId { get; set; }
    public Guid TargetWalletId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}
