using System.Net.Http.Json;
using Endava.TechCourse.BankApp.Shared;

namespace Endava.TechCourse.BankApp.Client.Pages.Wallet;

public partial class WalletCreation
{
    private List<CurrencyDto> currencies;
    private readonly WalletDto wallet = new();

    protected override async Task OnInitializedAsync()
    {
        currencies = await HttpClient.GetFromJsonAsync<List<CurrencyDto>>("api/currencies");
    }

    public async Task CreateWallet()
    {
        var response = await HttpClient.PostAsJsonAsync("api/wallets", wallet);

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/wallets");
        }
    }
}
