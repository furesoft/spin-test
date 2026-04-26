using System.Security.Claims;

namespace DotnetTest.Core.JWT;

public sealed class JwtPrincipal : ClaimsPrincipal
{
    public JwtPrincipal(IDictionary<string, object> payload)
    {
        var claims = payload.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
        var identity = new JwtIdentity(claims);
        AddIdentity(identity);
    }

    public bool HasPermission(string permission)
    {
        return HasClaim("permission", permission);
    }

    public string Username => FindFirst(ClaimTypes.NameIdentifier)?.Value;
}