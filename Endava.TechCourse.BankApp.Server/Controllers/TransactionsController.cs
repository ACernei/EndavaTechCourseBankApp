using Endava.TechCourse.BankApp.Application.Commands;
using Endava.TechCourse.BankApp.Application.Commands.CreateTransactionByCode;
using Endava.TechCourse.BankApp.Application.Commands.CreateTransactionByEmail;
using Endava.TechCourse.BankApp.Application.Queries.GetTransactions;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "User")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator mediator;

    public TransactionsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("UserId invalid");
        if (dto.TargetSearchMethod == TargetSearchMethod.None)
            return BadRequest("Nu poate fi cautata destinatia");
        if (!Guid.TryParse(dto.SourceId, out var sourceId))
            return BadRequest("SourceId invalid");
        if (!Guid.TryParse(dto.CurrencyId, out var currencyId))
            return BadRequest("CurrencyId invalid");

        IRequest<CommandStatus> command = dto.TargetSearchMethod switch
        {
            TargetSearchMethod.ByEmail => new CreateTransactionByEmailCommand
            {
                TransactionType = dto.TransactionType,
                SourceWalletId = sourceId,
                Amount = dto.Amount,
                CurrencyId = currencyId,
                Description = dto.Description,
                InitiatorUserId = userId,
                TargetEmail = dto.TargetSearchTerm
            },
            TargetSearchMethod.ByWalletCode => new CreateTransactionByCodeCommand
            {
                TransactionType = dto.TransactionType,
                SourceWalletId = sourceId,
                Amount = dto.Amount,
                CurrencyId = currencyId,
                Description = dto.Description,
                InitiatorUserId = userId,
                TargetWalletCode = dto.TargetSearchTerm
            }
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<List<TransactionDto>> GetTransactions()
    {
        var query = new GetTransactionsQuery();
        var transactions = await mediator.Send(query);

        var transactionDtos = transactions.Select(Mapper.Map).ToList();

        return transactionDtos;
    }
}
