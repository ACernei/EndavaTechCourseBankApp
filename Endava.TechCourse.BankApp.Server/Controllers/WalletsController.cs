using Endava.TechCourse.BankApp.Application.Commands.AddWallet;
using Endava.TechCourse.BankApp.Application.Commands.DeleteWallet;
using Endava.TechCourse.BankApp.Application.Commands.UpdateWallet;
using Endava.TechCourse.BankApp.Application.Queries.GetWalletById;
using Endava.TechCourse.BankApp.Application.Queries.GetWallets;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
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
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto)
    {
        var command = new AddWalletCommand
        {
            Type = walletDto.Type,
            Amount = walletDto.Amount,
            CurrencyId = Guid.Parse(walletDto.Currency.Id)
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<List<WalletDto>> GetWallets()
    {
        var query = new GetWalletsQuery();
        var wallets = await mediator.Send(query);

        var walletDtos = wallets.Select(Mapper.Map).ToList();

        return walletDtos;
    }

    [HttpGet("{id}")]
    public async Task<WalletDto> GetWalletById(Guid id)
    {
        var query = new GetWalletByIdQuery
        {
            Id = id
        };
        var wallet = await mediator.Send(query);

        var walletDto = Mapper.Map(wallet);

        return walletDto;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWallet(Guid id, [FromBody] WalletDto walletDto)
    {
        var command = new UpdateWalletCommand
        {
            Id = id,
            Type = walletDto.Type,
            Amount = walletDto.Amount,
            CurrencyId = Guid.Parse(walletDto.Currency.Id)
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
