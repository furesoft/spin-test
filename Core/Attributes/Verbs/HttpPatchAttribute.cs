using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core.Attributes.Verbs;

public class HttpPatchAttribute(string route) : HttpRouteAttribute(route, HttpMethod.Patch);