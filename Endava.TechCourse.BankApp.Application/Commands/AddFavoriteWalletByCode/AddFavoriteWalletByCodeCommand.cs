using MediatR;

namespace Endava.TechCourse.BankApp.Application.Commands.AddFavoriteWalletByCode;

public class AddFavoriteWalletByCodeCommand : IRequest<CommandStatus>
{
    public string WalletCode { get; set; }
    public Guid UserId { get; set; }
}
