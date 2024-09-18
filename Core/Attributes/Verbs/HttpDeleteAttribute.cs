using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace DotnetTest.Core.Attributes.Verbs;

public class HttpDeleteAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Delete);