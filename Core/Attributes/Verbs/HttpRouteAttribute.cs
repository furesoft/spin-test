using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace DotnetTest.Core.Attributes.Verbs;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HttpRouteAttribute(string route, HttpMethod method) : Attribute
{
    public string Route { get; } = route;
    public HttpMethod Method { get; } = method;
}