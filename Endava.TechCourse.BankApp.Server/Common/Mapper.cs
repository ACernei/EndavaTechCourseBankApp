using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Shared;

namespace Endava.TechCourse.BankApp.Server.Common;

public static class Mapper
{
    public static WalletDto Map(Wallet source)
    {
        return new WalletDto
        {
            Id = source.Id.ToString(),
            CreationDate = source.TimeStamp,
            Type = source.Type,
            Amount = source.Amount,
            Currency = Map(source.Currency)
        };
    }

    public static CurrencyDto Map(Currency source)
    {
        return new CurrencyDto
        {
            Id = source.Id.ToString(),
            Name = source.Name,
            Code = source.Code,
            ChangeRate = source.ChangeRate
        };
    }
}