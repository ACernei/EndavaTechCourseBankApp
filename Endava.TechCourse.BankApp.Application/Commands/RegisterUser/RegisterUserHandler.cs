using Endava.TechCourse.BankApp.Domain.Enums;
using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, CommandStatus>
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<User> userManager;

    public RegisterUserHandler(ApplicationDbContext context, UserManager<User> userManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(userManager);

        this.context = context;
        this.userManager = userManager;
    }

    public async Task<CommandStatus> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userEmailExists = await context.Users.AnyAsync(user => user.Email == request.Email, cancellationToken);
        if (userEmailExists)
            return CommandStatus.Failed("Utilizatorul deja exista");

        var userRole = await context.Users.AnyAsync(cancellationToken) ? UserRole.User : UserRole.Admin;

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            return CommandStatus.Failed("Utilizatorul nu a putut fi creat");

        var roleResult = await userManager.AddToRoleAsync(user, userRole.ToString());
        if (!roleResult.Succeeded)
            return CommandStatus.Failed("Utilizatorul nu a putut fi inregistrat");

        return new CommandStatus();
    }
}
