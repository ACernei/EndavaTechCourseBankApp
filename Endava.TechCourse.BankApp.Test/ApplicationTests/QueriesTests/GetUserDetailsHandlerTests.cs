using Endava.TechCourse.BankApp.Application.Queries.GetUserDetails;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.QueriesTests;

public class GetUserDetailsHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GetUserDetailsHandler).GetConstructors());
    }
}
