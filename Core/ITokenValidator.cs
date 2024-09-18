namespace DotnetTest.Core;

public interface ITokenValidator
{
    bool Validate(string token);
}