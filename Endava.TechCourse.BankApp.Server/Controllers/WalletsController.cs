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
    public async Task CreateWallet([FromBody] WalletDto walletDto)
    {
        // searching by 'x.Id.ToString()'
        // this works in prod, but fails in UnitTests (FirstOrDefault/Async also fails in UnitTests)
        // var currency = await context.Currencies.FirstAsync(x => x.Id.ToString() == walletDto.Currency.Id);

        // searching by 'x.Id' (after converting the request string to Guid)
        // this works in prod, also in UnitTests
        if (!Guid.TryParse(walletDto.Currency.Id, out var requestCurrencyId))
            throw new InvalidOperationException($"Received invalid Id: {walletDto.Currency.Id}");
        var currency = await context.Currencies.FirstAsync(x => x.Id == requestCurrencyId);

        var wallet = new Wallet
        {
            Type = walletDto.Type,
            Amount = walletDto.Amount,
            Currency = currency
        };

        context.Wallets.Add(wallet);
        await context.SaveChangesAsync();
    }

    [HttpGet]
    public List<WalletDto> GetWallets()
    {
        var walletDtos = context.Wallets
            .Include(x => x.Currency)
            .Select(WalletDto.From)
            .ToList();

        return walletDtos;
    }

    [HttpGet("{id}")]
    public async Task<WalletDto> GetWalletById(Guid id)
    {
        var walletDomain = await context.Wallets
            .Include(x => x.Currency)
            .FirstAsync(x => x.Id == id);
        var walletDto = WalletDto.From(walletDomain);

        return walletDto;
    }
}
