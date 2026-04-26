using DotnetTest.Core.Attributes;
using DotnetTest.Core.Attributes.Verbs;
using DotnetTest.Core;
using System.Security.Principal;
using DotnetTest.Core.JWT;
using JWT;
using JWT.Serializers;

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

    [HttpGet("/auth")]
    public string Auth()
    {
        var encoder = new JwtEncoder(new JWT.Algorithms.NoneAlgorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
        var payload = new JwtPayload
        {
            Iss = "furesoft",
            Iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Exp = DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds(),
            PreferredUsername = "filmee24",
            Roles = new List<string>() {"admin", "editor"}
        };

        return encoder.Encode(payload.Payload, "");
    }

    [HttpGet("/v/{username}")]
    //[Authorization("admin")]
    public string GetTestAuth(HttpContext context, string username, [Header("accept")] string accept)
    {
        var identity = context.Principal?.Identity?.Name ?? username;
        return "Willkommen " + identity + ", you sent accept header: " + accept;
    }

    [HttpPost("/body")]
    public string Body([RequestBody] string body)
    {
        return body;
    }
}