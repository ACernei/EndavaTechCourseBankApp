using Endava.TechCourse.BankApp.Application.Queries.GetCurrencies;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.QueriesTests;

public class GetAllCurrenciesHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GetCurrenciesHandler).GetConstructors());
    }
}