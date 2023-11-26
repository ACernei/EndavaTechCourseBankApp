using Endava.TechCourse.BankApp.Application.Commands.AddWallet;
using Endava.TechCourse.BankApp.Application.Commands.DeleteWallet;
using Endava.TechCourse.BankApp.Application.Commands.UpdateWallet;
using Endava.TechCourse.BankApp.Application.Queries.GetWalletById;
using Endava.TechCourse.BankApp.Application.Queries.GetWallets;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletsController : ControllerBase
{
    private readonly IMediator mediator;

    public WalletsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null)
            return BadRequest("UserId invalid");


        var command = new AddWalletCommand
        {
            WalletType = walletDto.WalletType,
            Amount = walletDto.Amount,
            CurrencyId = Guid.Parse(walletDto.Currency.Id),
            UserId = Guid.Parse(userIdClaim.Value)
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

        var query = new GetWalletsQuery
        {
            UserId = userId
        };

        var wallets = await mediator.Send(query);

        var walletDtos = wallets.Select(Mapper.Map).ToList();

        return walletDtos;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User")]
    public async Task<WalletDto> GetWalletById(Guid id)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new InvalidOperationException("UserId invalid");

        var query = new GetWalletByIdQuery
        {
            Id = id,
            UserId = userId
        };
        var wallet = await mediator.Send(query);

        var walletDto = Mapper.Map(wallet);

        return walletDto;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateWallet(Guid id, [FromBody] WalletDto walletDto)
    {
        if (!Guid.TryParse(walletDto.Currency.Id, out var currencyId))
            return BadRequest("CurrencyId invalid");

        var command = new UpdateWalletCommand
        {
            Id = id,
            WalletType = walletDto.WalletType,
            Amount = walletDto.Amount,
            CurrencyId = currencyId
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        var command = new DeleteWalletCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }
}
