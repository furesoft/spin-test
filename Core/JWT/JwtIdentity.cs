using System.Security.Claims;

namespace DotnetTest.Core.JWT;

public class JwtIdentity(IEnumerable<Claim> claims) : ClaimsIdentity(claims, "JWT");