using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using Endava.TechCourse.BankApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public WalletController(ApplicationDbContext dbContext)
    {
        context = dbContext;
    }

    [HttpPost]
    public IActionResult CreateWallet([FromBody] CreateWalletDto createWalletDto)
    {
        var wallet = new Wallet
        {
            Type = createWalletDto.Type,
            Amount = createWalletDto.Amount,
            Currency = new Currency()
            {
                Name = "EURO",
                CurrencyCode = "EUR",
                ChangeRate = 20
            }
        };

        context.Wallets.Add(wallet);
        context.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("getWallets")]
    public async Task<List<Wallet>> GetWallets()
    {
        return await context.Wallets
            .Include(x => x.Currency)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Wallet> GetWalletById(Guid id)
    {
        return await context.Wallets.FindAsync(id);
    }
}
