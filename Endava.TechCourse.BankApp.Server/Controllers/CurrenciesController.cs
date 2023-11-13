using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;
using Endava.TechCourse.BankApp.Application.Commands.UpdateCurrency;
using Endava.TechCourse.BankApp.Application.Queries.GetCurrencies;
using Endava.TechCourse.BankApp.Application.Queries.GetCurrencyById;
using Endava.TechCourse.BankApp.Server.Common;
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
        var query = new GetCurrenciesQuery();
        var currencies = await mediator.Send(query);

        var currencyDtos = currencies.Select(Mapper.Map).ToList();

        return currencyDtos;
    }

    [HttpGet("{id}")]
    public async Task<CurrencyDto> GetCurrencyById(Guid id)
    {
        var query = new GetCurrencyByIdQuery
        {
            Id = id
        };
        var currency = await mediator.Send(query);

        var currencyDto = Mapper.Map(currency);

        return currencyDto;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCurrency(Guid id, [FromBody] CurrencyDto currencyDto)
    {
        var command = new UpdateCurrencyCommand
        {
            Id = id,
            Name = currencyDto.Name,
            Code = currencyDto.Code,
            ChangeRate = currencyDto.ChangeRate
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurrency(Guid id)
    {
        var command = new DeleteCurrencyCommand
        {
            Id = id
        };
        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }
}
