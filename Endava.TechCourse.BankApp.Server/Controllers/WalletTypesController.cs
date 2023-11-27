using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;
using Endava.TechCourse.BankApp.Application.Commands.UpdateWalletType;
using Endava.TechCourse.BankApp.Application.Queries.GetWalletTypes;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletTypesController : ControllerBase
{
    private readonly IMediator mediator;

    public WalletTypesController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddWalletType([FromBody] WalletTypeDto walletTypeDto)
    {
        var command = new AddWalletTypeCommand
        {
            Name = walletTypeDto.Name,
            TransactionFee = walletTypeDto.TransactionFee
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<List<WalletTypeDto>> GetWalletTypes()
    {
        var query = new GetWalletTypesQuery();
        var walletTypes = await mediator.Send(query);

        var walletTypeDtos = walletTypes.Select(Mapper.Map).ToList();

        return walletTypeDtos;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateWalletType(Guid id, [FromBody] WalletTypeDto walletTypeDto)
    {
        var command = new UpdateWalletTypeCommand
        {
            Id = id,
            Name = walletTypeDto.Name,
            TransactionFee = walletTypeDto.TransactionFee
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteWalletType(Guid id)
    {
        var command = new DeleteWalletTypeCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }
}
