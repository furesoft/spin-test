using System.Collections.Specialized;
using System.Reflection;
using System.Security.Principal;
using Fermyon.Spin.Sdk;

namespace Project.Core;

public class HttpContext(HttpRequest request, HttpResponse response, MethodInfo method, Uri url)
{
    public HttpRequest Request { get; } = request;
    public HttpResponse Response { get; set; } = response;
    public MethodInfo Method { get; } = method;
    public object Controller { get; set; }
    public Uri Url { get; } = url;

    public NameValueCollection Query;

    public IPrincipal Identity { get; set; }
}