using Endava.TechCourse.BankApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Shared;

public class WalletDto
{
    public string Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public CurrencyDto Currency { get; set; }

    public static WalletDto From(Wallet source) => new()
    {
        Id = source.Id.ToString(),
        CreationDate = source.TimeStamp,
        Type = source.Type,
        Amount = source.Amount,
        Currency = CurrencyDto.From(source.Currency),
    };
}
