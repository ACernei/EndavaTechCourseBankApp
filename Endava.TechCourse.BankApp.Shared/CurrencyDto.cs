namespace Endava.TechCourse.BankApp.Shared;

public class CurrencyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal ChangeRate { get; set; }
    public bool CanBeRemoved { get; set; } = true;

    public override string ToString()
    {
        return Code;
    }
}
