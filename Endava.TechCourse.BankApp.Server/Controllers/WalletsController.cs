using Endava.TechCourse.BankApp.Application.Commands.AddWallet;
using Endava.TechCourse.BankApp.Application.Commands.DeleteWallet;
using Endava.TechCourse.BankApp.Application.Commands.SetMainWallet;
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
[Authorize(Roles = "User")]
public class WalletsController : ControllerBase
{
    private readonly IMediator mediator;

    public WalletsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("UserId invalid");
        if (!Guid.TryParse(walletDto.CurrencyId, out var currencyId))
            return BadRequest("CurrencyId invalid");
        if (!Guid.TryParse(walletDto.WalletTypeId, out var walletTypeId))
            return BadRequest("WalletTypeId invalid");

        var command = new AddWalletCommand
        {
            WalletTypeId = walletTypeId,
            Amount = walletDto.Amount,
            CurrencyId = currencyId,
            UserId = userId
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpPost]
    [Route("SetMainWallet/{walletId}")]
    public async Task<IActionResult> SetMainWallet(Guid walletId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("UserId invalid");

        var command = new SetMainWalletCommand
        {
            WalletId = walletId,
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

        var query = new GetWalletsQuery
        {
            UserId = userId
        };

        var wallets = await mediator.Send(query);

        var walletDtos = wallets.Select(Mapper.Map).ToList();

        return walletDtos;
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> UpdateWallet(Guid id, [FromBody] WalletDto walletDto)
    {
        if (!Guid.TryParse(walletDto.CurrencyId, out var currencyId))
            return BadRequest("CurrencyId invalid");
        if (!Guid.TryParse(walletDto.WalletTypeId, out var walletTypeId))
            return BadRequest("WalletTypeId invalid");

        var command = new UpdateWalletCommand
        {
            Id = id,
            WalletTypeId = walletTypeId,
            Amount = walletDto.Amount,
            CurrencyId = currencyId
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
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
