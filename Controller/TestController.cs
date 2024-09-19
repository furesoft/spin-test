using DotnetTest.Core.Attributes;
using DotnetTest.Core.Attributes.Verbs;
using DotnetTest.Core;
using System.Security.Principal;

namespace DotnetTest.Controller;

public class TestController
{
    [HttpGet("/")]
    public string GetIndex()
    {
        return "Hello world";
    }

    [HttpGet("/test")]
    public string GetTest()
    {
        return "it does work";
    }

    [HttpGet("/v/{username}")]
    [Authorization("admin")]
    public string GetTestAuth(HttpContext context, [PathParameter] string username, [Header("accept")] string password, [PathQuery("q")] string q)
    {
        var identity = (GenericIdentity)context.Principal.Identity;
        return "Willkommen " + identity.Name;
    }

    [HttpGet("/auth")]
    public string Auth()
    {
        return "aGVsbG93b3JsZA==";
    }

    [HttpPost("/body")]
    public string Body([RequestBody] string body)
    {
        return body;
    }
}