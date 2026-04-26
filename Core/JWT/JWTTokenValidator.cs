using System.Security.Principal;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace DotnetTest.Core.JWT;

public class JWTTokenValidator : ITokenHandler
{
    public JWTTokenValidator() {}

    public bool Validate(string token, out IPrincipal principal)
    {
        IJsonSerializer serializer = new JsonNetSerializer();
        IDateTimeProvider provider = new UtcDateTimeProvider();
        IJwtValidator validator = new JwtValidator(serializer, provider);
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new NoneAlgorithm());

        var payload = decoder.DecodeToObject(token);
        principal = new JwtPrincipal(payload);

        return true;
    }
}