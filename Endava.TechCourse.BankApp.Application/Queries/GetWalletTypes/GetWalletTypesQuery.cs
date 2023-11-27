using Endava.TechCourse.BankApp.Domain.Models;
using MediatR;

namespace Endava.TechCourse.BankApp.Application.Queries.GetWalletTypes;

public class GetWalletTypesQuery : IRequest<List<WalletType>>
{
}
