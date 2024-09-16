using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes.Verbs;

public class HttpDeleteAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Delete);