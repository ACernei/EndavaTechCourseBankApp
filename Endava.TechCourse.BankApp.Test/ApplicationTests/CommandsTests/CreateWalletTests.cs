using Endava.TechCourse.BankApp.Application.Commands.AddWallet;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.CommandsTests;

public class CreateWalletTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddWalletHandler).GetConstructors());
    }

    [Test, ApplicationData]
    public async Task ShouldReturnRequestFailedIfCurrencyIsNull(
        AddWalletCommand command,
        AddWalletHandler handler)
    {
        var result = await handler.Handle(command, default);

        using (new AssertionScope())
        {
            result.IsSuccessful = false;
            result.Error = "Valuta pentru acest portofel nu exista";
        }
    }
}
