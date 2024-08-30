using Microsoft.AspNetCore.Authentication;

namespace ProductsWebAPI.Security;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}