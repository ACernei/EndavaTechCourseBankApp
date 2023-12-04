using Endava.TechCourse.BankApp.Application.Queries.GetWallets;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.QueriesTests;

public class GetWalletsHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GetWalletsHandler).GetConstructors());
    }
}
