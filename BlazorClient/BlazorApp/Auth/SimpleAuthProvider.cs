using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using DTOs.LogInInfo;
using DTOs.UserDtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jSRuntime;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jSRuntime)
    {
        this.httpClient = httpClient;
        this.jSRuntime = jSRuntime;
    }

    public async Task LoginAsync(string email, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "api/auth/login",
            new UserLoginDto(email, password));
        
        string content = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine($"Content-Type: {response.Content.Headers.ContentType}");
        Console.WriteLine($"Response: '{content}'");
        
        if(!response.IsSuccessStatusCode)
        {
             Console.WriteLine($"Login failed: {(int)response.StatusCode} {response.StatusCode}");
             Console.WriteLine($"Response: {content}");
            throw new Exception(content);
        }
        if(string.IsNullOrWhiteSpace(content))
        {
            throw new Exception("Login API returned an empty response.");
        }
    
        LoginResponseDto? loginResponse;
        try
        {
            loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
        catch(JsonException ex)
        {
            throw new Exception(
                $"Login API did not return valid JSON. Response was: '{content}'",
            ex);
        }

        if (loginResponse == null)
        {
            throw new Exception("Login response could not be deserialized.");
        }
        if(string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            throw new Exception(
                "Login response did not contain an access token."
            );
        }
        if(loginResponse.User == null)
        {
            throw new Exception("Login response did not contain a user.");
        }
        UserDto user = loginResponse.User;
        string token = loginResponse.Token;
        //Store JWT token
        await jSRuntime.InvokeVoidAsync("sessionStorage.setItem", "accessToken", token);

        //Store current user
         string serialisedData = JsonSerializer.Serialize(user);
         await jSRuntime.InvokeVoidAsync("sessionStorage.setItem","currentUser",serialisedData);

         //Create claims for Blazor authentication state
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "api/auth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal))
        );
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
     // return new AuthenticationState(currentClaimsPrincipal ?? new ());
     string userAsJson = "";
     try
        {
            userAsJson = await jSRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        }
        catch(InvalidOperationException)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        if(string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        UserDto? userDto;
        try
        {
            userDto = JsonSerializer.Deserialize<UserDto>(userAsJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
        }
        catch (JsonException)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity())
            );
        }
        if(userDto == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()
                )
            );
        }
        if(userDto == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()
                )
            );
        }
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Name, userDto.FullName),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
        };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "api/auth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(claimsPrincipal);
    }

    public async Task LogOut()
    {
        /*
        currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
        */
        await jSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentUser");
        await jSRuntime.InvokeVoidAsync("sessionStorage.removeItem","accessToken");
        ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}
