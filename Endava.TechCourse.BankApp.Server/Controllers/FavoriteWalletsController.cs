using Endava.TechCourse.BankApp.Application.Commands.AddFavoriteWalletByCode;
using Endava.TechCourse.BankApp.Application.Queries.GetFavoriteWallets;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "User")]
public class FavoriteWalletsController : ControllerBase
{
    private readonly IMediator mediator;

    public FavoriteWalletsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddFavoriteWalletByCode([FromBody] WalletDto walletDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("UserId invalid");

        var command = new AddFavoriteWalletByCodeCommand
        {
            WalletCode = walletDto.Code,
            UserId = userId
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<List<WalletDto>> GetWallets()
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new InvalidOperationException("UserId invalid");

        var query = new GetFavoriteWalletsQuery
        {
            UserId = userId
        };

        var wallets = await mediator.Send(query);

        var walletDtos = wallets.Select(Mapper.Map).ToList();

        return walletDtos;
    }

    // [HttpPost]
    // public async Task<IActionResult> DeleteFavoriteWalletByCode([FromBody] WalletDto walletDto)
    // {
    //     var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
    //     if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
    //         return BadRequest("UserId invalid");
    //
    //     var command = new AddFavoriteWalletByCodeCommand
    //     {
    //         WalletCode = walletDto.Code,
    //         UserId = userId
    //     };
    //
    //     var result = await mediator.Send(command);
    //
    //     return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    // }
}
