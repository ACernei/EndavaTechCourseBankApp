using Endava.TechCourse.BankApp.Domain.Models;
using MediatR;

namespace Endava.TechCourse.BankApp.Application.Queries.GetFavoriteWallets;

public class GetFavoriteWalletsQuery : IRequest<List<Wallet>>
{
    public Guid UserId { get; set; }
}
