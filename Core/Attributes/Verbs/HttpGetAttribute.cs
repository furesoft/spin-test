using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes.Verbs;
public class HttpGetAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Get);