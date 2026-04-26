using System.Security.Principal;

namespace DotnetTest.Core;

public interface ITokenHandler
{
    bool Validate(string token, out IPrincipal principal);
}