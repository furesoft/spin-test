using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes.Verbs;

public class HttpPostAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Post);