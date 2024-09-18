using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace DotnetTest.Core.Attributes.Verbs;

public class HttpPutAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Put);