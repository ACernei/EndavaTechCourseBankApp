using Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.CommandsTests;

public class DeleteCurrencyHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DeleteCurrencyHandler).GetConstructors());
    }
}
