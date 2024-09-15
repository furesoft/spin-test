using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class HttpRouteAttribute : Attribute
{
    public string Route { get; }
    public HttpMethod Method { get; }

    public HttpRouteAttribute(string route, HttpMethod method)
    {
        Route = route;
        Method = method;
    }
}