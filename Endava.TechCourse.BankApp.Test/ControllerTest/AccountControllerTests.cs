using Endava.TechCourse.BankApp.Server.Controllers;

namespace Endava.TechCourse.BankApp.Test.ControllerTest;

public class AccountControllerTests
{
    [Test]
    [ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AccountController).GetConstructors());
    }
}
