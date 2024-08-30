using Microsoft.AspNetCore.Authentication;

namespace ProductsWebAPI.Security;

public class FakeAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}