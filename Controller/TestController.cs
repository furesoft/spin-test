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
    //[Authorization("admin")]
    public string GetTestAuth(HttpContext context, string username, [Header("accept")] string accept)
    {
        var identity = context.Principal?.Identity?.Name ?? username;
        return "Willkommen " + identity + ", you sent accept header: " + accept;
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