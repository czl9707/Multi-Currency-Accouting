namespace Accountant.Services.Middleware;

public class RepsonseCorsHeaderMiddleware
{
    private RequestDelegate _next;
    public RepsonseCorsHeaderMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        await _next(context);
    }
}
