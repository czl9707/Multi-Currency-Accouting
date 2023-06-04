using System.Text;

namespace Accountant.Services.Middleware;

public class EnableRequestBodyBufferingMiddleware
{
    private RequestDelegate _next;
    public EnableRequestBodyBufferingMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        await _next(context);
    }
}

public static class RequestBodyBuffering
{
    public static async Task<string> GetRawBodyAsync(
        this HttpRequest request,
        Encoding? encoding = null)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        request.Body.Position = 0;
        var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
        var body = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;
        return body;
    }
}