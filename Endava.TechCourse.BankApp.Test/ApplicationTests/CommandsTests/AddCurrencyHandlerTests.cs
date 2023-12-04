using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.CommandsTests;

public class AddCurrencyHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddCurrencyHandler).GetConstructors());
    }
}
