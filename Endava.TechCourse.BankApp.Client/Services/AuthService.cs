using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Endava.TechCourse.BankApp.Client.Common;
using Endava.TechCourse.BankApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace Endava.TechCourse.BankApp.Client.Services;

public class AuthService : AuthenticationStateProvider
{
    private readonly AuthenticationState anonymous;
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorage;

    public AuthService(ILocalStorageService localStorage, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(localStorage);
        ArgumentNullException.ThrowIfNull(httpClient);

        this.localStorage = localStorage;
        this.httpClient = httpClient;
        anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(token))
            return anonymous;

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser =
            new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<AuthResult> Register(RegisterDto data)
    {
        var result = await httpClient.PostAsJsonAsync("api/account/register", data);

        if (result.StatusCode != HttpStatusCode.OK)
            return new AuthResult { Status = false, Message = string.Empty };

        return new AuthResult { Status = true };
    }

    public async Task<AuthResult> Login(LoginDto data)
    {
        var result = await httpClient.PostAsJsonAsync("api/account/login", data);

        if (!result.IsSuccessStatusCode)
            return new AuthResult { Status = false, Message = await result.Content.ReadAsStringAsync() };

        var token = await result.Content.ReadAsStringAsync();
        await localStorage.SetItemAsync("authToken", token);
        NotifyUserAuthentication(token);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthResult { Status = true };
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        var authState = Task.FromResult(anonymous);
        NotifyAuthenticationStateChanged(authState);
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var token = await localStorage.GetItemAsync<string>("authToken");

        return !string.IsNullOrWhiteSpace(token);
    }
}