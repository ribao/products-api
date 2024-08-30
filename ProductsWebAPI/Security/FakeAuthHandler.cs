using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ProductsWebAPI.Security;

public class FakeAuthHandler : AuthenticationHandler<FakeAuthHandlerOptions>
{
    public const string ApiKey = "ApiKey";

    public const string AuthenticationScheme = "FakeAuthHandlerScheme";
    
    public FakeAuthHandler(
        IOptionsMonitor<FakeAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue(ApiKey, out var userId))
            return Task.FromResult(AuthenticateResult.Fail("Invalid user id"));
       
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.NameIdentifier, userId[0])
        };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}