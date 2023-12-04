using Endava.TechCourse.BankApp.Application.Commands.CreateTransactionByCode;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.CommandsTests;

public class TransferFoundsHandlerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CreateTransactionByCodeHandler).GetConstructors());
    }
}
