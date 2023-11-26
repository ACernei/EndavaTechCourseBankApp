using Endava.TechCourse.BankApp.Application.Commands.AddWallet;

namespace Endava.TechCourse.BankApp.Server.Composition;

public static class PresentationModule
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            config.RegisterServicesFromAssembly(typeof(AddWalletCommand).Assembly);
        });

        return services;
    }
}
