using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using Endava.TechCourse.BankApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public CurrenciesController(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    [HttpPost]
    public IActionResult CreateCurrency([FromBody] CurrencyDto currencyDto)
    {
        var currency = new Currency
        {
            Name = currencyDto.Name,
            CurrencyCode = currencyDto.Code,
            ChangeRate = currencyDto.ChangeRate
        };

        context.Currencies.Add(currency);
        context.SaveChanges();

        return Ok();
    }

    [HttpGet]
    public IActionResult GetCurrencies()
    {
        var currencyDtos = context.Currencies
            .Select(CurrencyDto.From)
            .ToList();

        return Ok(currencyDtos);
    }
}
