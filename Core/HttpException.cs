using System.Net;

namespace DotnetTest.Core;

public class HttpException(HttpStatusCode status, string? message) : Exception(message)
{
    public HttpStatusCode Status { get; } = status;
}