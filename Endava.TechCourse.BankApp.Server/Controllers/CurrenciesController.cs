using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator mediator;

    public CurrenciesController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddCurrency([FromBody] CurrencyDto currencyDto)
    {
        var command = new AddCurrencyCommand
        {
            Name = currencyDto.Name,
            Code = currencyDto.Code,
            ChangeRate = currencyDto.ChangeRate
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<List<CurrencyDto>> GetCurrencies()
    {
        return new();
    }
}
