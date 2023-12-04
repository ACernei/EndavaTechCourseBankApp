using Endava.TechCourse.BankApp.Application.Commands;
using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Server.Controllers;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using NSubstitute;

namespace Endava.TechCourse.BankApp.Test.ControllerTest;

public class CurrencyControllerTests
{
    [Test, ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CurrenciesController).GetConstructors());
    }

    [Test, ApplicationData]
    public async Task ShouldSaveCurrency(
        [Frozen] IMediator mediator,
        [Greedy] CurrenciesController controller,
        CurrencyDto dto)
    {
        mediator.Send(Arg.Any<AddCurrencyCommand>()).Returns(new CommandStatus());

        await controller.AddCurrency(dto);

        await mediator.Received(1).Send(Arg.Is<AddCurrencyCommand>(x
            => x.Name == dto.Name
               && x.ChangeRate == dto.ChangeRate
               && x.Code == dto.Code));
    }
}
