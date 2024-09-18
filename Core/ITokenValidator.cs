namespace Project.Core;

public interface ITokenValidator
{
    bool Validate(string token);
}