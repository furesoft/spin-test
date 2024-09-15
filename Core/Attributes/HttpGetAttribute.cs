using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes;
public class HttpGetAttribute : HttpRouteAttribute
{
    public HttpGetAttribute(string route) : base(route, HttpMethod.Get) { }
}