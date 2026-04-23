using System.Security.Principal;

namespace DotnetTest.Core;

public class DefaultTokenHandler : ITokenHandler
{
    public DefaultTokenHandler() {}

    public bool Handle(string token, out IPrincipal principal)
    {
        principal = new GenericPrincipal(new GenericIdentity("admin"), ["admin"]);
        return true;
    }
}