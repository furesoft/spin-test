using System.Security.Principal;

namespace DotnetTest.Core;

public interface ITokenHandler
{
    bool Handle(string token, out IPrincipal principal);
}