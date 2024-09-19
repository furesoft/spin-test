using DotnetTest.Core.Attributes;
using DotnetTest.Core.Attributes.Verbs;

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
    public string GetTestAuth([PathParameter] string username, [Header("accept")] string password, [PathQuery("q")] string q)
    {
        return "Erfolgreich eingeloogt";
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