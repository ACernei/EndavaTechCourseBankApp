using Endava.TechCourse.BankApp.Application.Commands.CreateTransaction;
using Endava.TechCourse.BankApp.Application.Queries.GetTransactions;
using Endava.TechCourse.BankApp.Server.Common;
using Endava.TechCourse.BankApp.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.TechCourse.BankApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator mediator;

    public TransactionsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        this.mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("UserId invalid");

        if (!Guid.TryParse(dto.SourceId, out var sourceId))
            return BadRequest("SourceId invalid");
        if (!Guid.TryParse(dto.TargetId, out var targetId))
            return BadRequest("TargetId invalid");
        if (!Guid.TryParse(dto.CurrencyId, out var currencyId))
            return BadRequest("CurrencyId invalid");

        var command = new CreateTransactionCommand
        {
            TransactionType = dto.TransactionType,
            SourceWalletId = sourceId,
            TargetWalletId = targetId,
            Amount = dto.Amount,
            CurrencyId = currencyId,
            Description = dto.Description,
            InitiatorUserId = userId
        };

        var result = await mediator.Send(command);

        return result.IsSuccessful ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<List<TransactionDto>> GetTransactions()
    {
        var query = new GetTransactionsQuery();
        var transactions = await mediator.Send(query);

        var transactionDtos = transactions.Select(Mapper.Map).ToList();

        return transactionDtos;
    }
}
