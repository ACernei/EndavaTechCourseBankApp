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
            Code = source.Code,
            UserEmail = source.User.Email,
            WalletTypeId = source.WalletTypeId.ToString(),
            Amount = source.Amount,
            CurrencyId = source.CurrencyId.ToString(),
            IsMain = source.IsMain(),
            TransactionFee = source.WalletType.TransactionFee
        };
    }

    public static WalletTypeDto Map(WalletType source)
    {
        return new WalletTypeDto
        {
            Id = source.Id.ToString(),
            Name = source.Name,
            TransactionFee = source.TransactionFee
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

    public static TransactionDto Map(Transaction source)
    {
        return new TransactionDto
        {
            TransactionType = source.TransactionType,
            SourceId = source.SourceId.ToString(),
            TargetId = source.TargetId.ToString(),
            Amount = source.Amount,
            Description = source.Description
        };
    }
}
