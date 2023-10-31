using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using Endava.TechCourse.BankApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletsController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public WalletsController(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto)
    {
        var currency = await this.context.Currencies.FirstOrDefaultAsync(x => x.Id.ToString() == walletDto.Currency.Id);
        var wallet = new Wallet
        {
            Type = walletDto.Type,
            Amount = walletDto.Amount,
            Currency = currency,
        };

        context.Wallets.Add(wallet);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public IActionResult GetWallets()
    {
        var walletDtos = context.Wallets
            .Include(x => x.Currency)
            .Select(WalletDto.From)
            .ToList();

        return Ok(walletDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        var walletDomain = await context.Wallets
            .Include(x => x.Currency)
            .FirstOrDefaultAsync(x => x.Id == id);
        var walletDto = WalletDto.From(walletDomain);

        return Ok(walletDto);
    }
}
