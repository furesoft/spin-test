namespace DotnetTest.Core.JWT;

public class JwtPayload
{
    private readonly Dictionary<string, object?> _payload = [];

    public Dictionary<string, object?> Payload => _payload;

    public string? Iss
    {
        get => _payload["iss"] as string;
        set => _payload["iss"] = value;
    }

    public string? Sub
    {
        get => _payload["sub"] as string;
        set => _payload["sub"] = value;
    }

    public object? Aud
    {
        get => _payload["aud"];
        set => _payload["aud"] = value;
    }

    public long? Exp
    {
        get => _payload["exp"] as long?;
        set => _payload["exp"] = value;
    }

    public long? Nbf
    {
        get => _payload["nbf"] as long?;
        set => _payload["nbf"] = value;
    }

    public long? Iat
    {
        get => _payload["iat"] as long?;
        set => _payload["iat"] = value;
    }

    public string? Jti
    {
        get => _payload["jti"] as string;
        set => _payload["jti"] = value;
    }

    public string? Name
    {
        get => _payload["name"] as string;
        set => _payload["name"] = value;
    }

    public string? Email
    {
        get => _payload["email"] as string;
        set => _payload["email"] = value;
    }

    public string? PreferredUsername
    {
        get => _payload["preferred_username"] as string;
        set => _payload["preferred_username"] = value;
    }

    public List<string>? Roles
    {
        get => _payload["roles"] as List<string>;
        set => _payload["roles"] = value;
    }

    public List<string>? Groups
    {
        get => _payload["groups"] as List<string>;
        set => _payload["groups"] = value;
    }

    public List<string>? Permissions
    {
        get => _payload["permissions"] as List<string>;
        set => _payload["permissions"] = value;
    }

    public string? Scope
    {
        get => _payload["scope"] as string;
        set => _payload["scope"] = value;
    }

    public void AddClaim(string name, object? value)
    {
        _payload[name] = value;
    }
}