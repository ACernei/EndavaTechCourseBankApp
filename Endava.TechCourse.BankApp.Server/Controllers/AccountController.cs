using Endava.TechCourse.BankApp.Application.Commands.LoginUser;
using Endava.TechCourse.BankApp.Application.Commands.RegisterUser;
using Endava.TechCourse.BankApp.Application.Queries.GetUserDetails;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly JwtService jwtService;
    private readonly IMediator mediator;

    public AccountController(IMediator mediator, JwtService jwtService)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(jwtService);

        this.mediator = mediator;
        this.jwtService = jwtService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var registerCommand = new RegisterUserCommand
        {
            Username = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Password = dto.Password,
            Email = dto.Email
        };

        var result = await mediator.Send(registerCommand);
        return result.IsSuccessful ? Ok() : BadRequest(new { result.Error });
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var loginCommand = new LoginUserCommand
        {
            Username = dto.Username,
            Password = dto.Password
        };

        var result = await mediator.Send(loginCommand);

        if (!result.IsSuccessful)
            return BadRequest(result.Error);

        var userDetailsQuery = new GetUserDetailsQuery
        {
            Username = dto.Username
        };

        var userDetails = await mediator.Send(userDetailsQuery);

        var jwtToken = jwtService.CreateAuthToken(userDetails.Id, userDetails.Username, userDetails.Roles);

        Response.Cookies.Append(Constants.TokenCookieName, jwtToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.MaxValue
        });

        return Ok(jwtToken);
    }
}
