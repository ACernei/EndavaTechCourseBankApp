using Endava.TechCourse.BankApp.Application.Queries.GetTransactions;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.QueriesTests;

public class GetTransactionsHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GetTransactionsHandler).GetConstructors());
    }
}
