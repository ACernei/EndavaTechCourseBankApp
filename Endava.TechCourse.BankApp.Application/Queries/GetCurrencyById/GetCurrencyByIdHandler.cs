using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Endava.TechCourse.BankApp.Application.Queries.GetCurrencyById;

public class GetCurrencyByIdHandler : IRequestHandler<GetCurrencyByIdQuery, Currency>
{
    private readonly ApplicationDbContext context;

    public GetCurrencyByIdHandler(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    public async Task<Currency> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        var currency = await context.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return currency;
    }
}
