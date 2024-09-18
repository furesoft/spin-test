using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace DotnetTest.Core.Attributes.Verbs;
public class HttpGetAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Get);