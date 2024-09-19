namespace DotnetTest.Core;

public class DefaultTokenValidator : ITokenValidator
{
    public DefaultTokenValidator() {}
    public bool Validate(string token)
    {
        return token == "aGVsbG93b3JsZA==";
    }
}