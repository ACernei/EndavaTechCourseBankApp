using Endava.TechCourse.BankApp.Domain.Models;

namespace Endava.TechCourse.BankApp.Shared;

public class CurrencyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal ChangeRate { get; set; }

    public static CurrencyDto From(Currency source) => new()
    {
        Id = source.Id.ToString(),
        Name = source.Name,
        Code = source.CurrencyCode,
        ChangeRate = source.ChangeRate
    };

    public override string ToString() => Code;
}
