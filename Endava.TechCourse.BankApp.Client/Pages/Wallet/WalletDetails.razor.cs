using System.Net.Http.Json;
using Endava.TechCourse.BankApp.Shared;
using Microsoft.AspNetCore.Components;

namespace Endava.TechCourse.BankApp.Client.Pages.Wallet;

public partial class WalletDetails
{
    private WalletDto wallet;

    [Parameter] public string WalletId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        wallet = await HttpClient.GetFromJsonAsync<WalletDto>($"/api/wallets/{WalletId}");
    }
}
