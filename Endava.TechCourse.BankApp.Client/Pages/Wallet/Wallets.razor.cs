using System.Net.Http.Json;
using Endava.TechCourse.BankApp.Shared;

namespace Endava.TechCourse.BankApp.Client.Pages.Wallet;

public partial class Wallets
{
    private List<WalletDto> wallets;

    protected override async Task OnInitializedAsync()
    {
        wallets = await HttpClient.GetFromJsonAsync<List<WalletDto>>("api/wallets");
    }
}
