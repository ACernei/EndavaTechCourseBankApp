using AutoFixture.Idioms;
using Endava.TechCourse.BankApp.Domain.Models;
using Endava.TechCourse.BankApp.Infrastructure.Persistence;
using Endava.TechCourse.BankApp.Server.Controllers;
using Endava.TechCourse.BankApp.Shared;
using Endava.TechCourse.BankApp.Test.Common;
using FluentAssertions;

namespace Endava.TechCourse.BankApp.Test.ControllerTest;

public class WalletControllerTests
{
    [Test]
    [ApplicationData]
    public void CanCreateInstance(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(WalletsController).GetConstructors());
    }

    [Test]
    [ApplicationData]
    public void ShouldGetWallets(
        [Frozen] ApplicationDbContext context,
        [Greedy] WalletsController controller,
        Wallet firstWallet,
        Wallet secondWallet)
    {
        //Arrange
        context.Wallets.AddRange(firstWallet, secondWallet);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        //Act
        var result = controller.GetWallets();

        //Assert
        result.Should().HaveCount(2);
    }

    [Test]
    [ApplicationData]
    public async Task ShouldGetWallet(
        [Frozen] ApplicationDbContext context,
        [Greedy] WalletsController controller,
        Wallet wallet)
    {
        //Arrange
        context.Wallets.AddRange(wallet);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        //Act
        var result = await controller.GetWalletById(wallet.Id);

        //Assert
        result.Id.Should().Be(wallet.Id.ToString());
    }

    [Test]
    [ApplicationData]
    public async Task ShouldThrowWhenWalletNotFound(
        [Greedy] WalletsController controller)
    {
        //Arrange

        //Act
        var act = async () => await controller.GetWalletById(Guid.NewGuid());

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    [ApplicationData]
    public async Task ShouldCreateWalletWithValidCurrency(
        [Frozen] ApplicationDbContext context,
        [Greedy] WalletsController controller,
        Currency currency,
        WalletDto walletDto)
    {
        //Arrange
        walletDto.Currency.Id = currency.Id.ToString();

        context.Currencies.AddRange(currency);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        //Act
        await controller.CreateWallet(walletDto);

        //Assert
        context.Wallets.Should().HaveCount(1);
    }

    [Test]
    [ApplicationData]
    public async Task CreateWalletShouldThrowWhenCurrencyNotFound(
        [Greedy] WalletsController controller,
        WalletDto walletDto)
    {
        //Arrange
        walletDto.Currency.Id = Guid.NewGuid().ToString();

        //Act
        var act = async () => await controller.CreateWallet(walletDto);

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    [ApplicationData]
    public async Task CreateWalletShouldThrowWhenInvalidCurrencyId(
        [Greedy] WalletsController controller,
        WalletDto walletDto)
    {
        //Arrange

        //Act
        var act = async () => await controller.CreateWallet(walletDto);

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
