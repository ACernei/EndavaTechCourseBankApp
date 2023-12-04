using Endava.TechCourse.BankApp.Application.Commands;

namespace Endava.TechCourse.BankApp.Test.ApplicationTests.CommandsTests;

public class CommandStatusTests
{
    [Test, ApplicationData]
    public void ShouldReturnIsSuccessfulTrue()
    {
        var result = new CommandStatus();

        using (new AssertionScope())
        {
            result.IsSuccessful.Should().Be(true);
            result.Error.Should().Be(string.Empty);
        }
    }

    [Test, ApplicationData]
    public void ShouldFailed(string errorMessage)
    {
        var result = CommandStatus.Failed(errorMessage);

        using (new AssertionScope())
        {
            result.IsSuccessful.Should().Be(false);
            result.Error.Should().Be(errorMessage);
        }
    }
}
