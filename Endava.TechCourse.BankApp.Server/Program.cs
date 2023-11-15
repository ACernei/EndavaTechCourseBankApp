using Endava.TechCourse.BankApp.Application.Commands.AddCurrency;
using Endava.TechCourse.BankApp.Application.Commands.AddWallet;
using Endava.TechCourse.BankApp.Application.Commands.DeleteCurrency;
using Endava.TechCourse.BankApp.Application.Commands.DeleteWallet;
using Endava.TechCourse.BankApp.Application.Commands.UpdateCurrency;
using Endava.TechCourse.BankApp.Application.Commands.UpdateWallet;
using Endava.TechCourse.BankApp.Application.Queries.GetCurrencies;
using Endava.TechCourse.BankApp.Application.Queries.GetCurrencyById;
using Endava.TechCourse.BankApp.Application.Queries.GetWalletById;
using Endava.TechCourse.BankApp.Application.Queries.GetWallets;
using Endava.TechCourse.BankApp.Infrastructure;
using Endava.TechCourse.BankApp.Server.Composition;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddInfrastructure(configuration);
builder.Services.AddJwtIdentity(configuration);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.RegisterServicesFromAssembly(typeof(GetWalletsQuery).Assembly);
    config.RegisterServicesFromAssembly(typeof(GetCurrenciesQuery).Assembly);
    config.RegisterServicesFromAssembly(typeof(GetWalletByIdQuery).Assembly);
    config.RegisterServicesFromAssembly(typeof(GetCurrencyByIdQuery).Assembly);
    config.RegisterServicesFromAssembly(typeof(AddWalletCommand).Assembly);
    config.RegisterServicesFromAssembly(typeof(AddCurrencyCommand).Assembly);
    config.RegisterServicesFromAssembly(typeof(UpdateWalletCommand).Assembly);
    config.RegisterServicesFromAssembly(typeof(UpdateCurrencyCommand).Assembly);
    config.RegisterServicesFromAssembly(typeof(DeleteWalletCommand).Assembly);
    config.RegisterServicesFromAssembly(typeof(DeleteCurrencyCommand).Assembly);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
