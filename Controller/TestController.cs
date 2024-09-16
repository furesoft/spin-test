using Project.Core.Attributes;
using Project.Core.Attributes.Verbs;

namespace Project.Controller;

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
    [Authorization("Admin")]
    public string GetTestAuth([PathParameter] string username, [Header("accept")] string password, [PathQuery("q")] string q)
    {
        return q;
    }
}