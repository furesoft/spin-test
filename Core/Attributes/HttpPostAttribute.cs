using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes;

public class HttpPostAttribute : HttpRouteAttribute
{
    public HttpPostAttribute(string route) : base(route, HttpMethod.Post) { }
}