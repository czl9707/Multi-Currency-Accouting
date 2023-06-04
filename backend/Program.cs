using Accountant.Models;
using Accountant.Services.DB;
using Accountant.Services.Middleware;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        AddServices(builder.Services);

        var app = builder.Build();
        
        AddMiddleWares(app);
        app.MapControllers();
        await app.RunAsync();
    }

    private static void AddServices(IServiceCollection services){
        AddHelperServices(services);
        AddContext(services);
        AddCors(services);
    }

    private static void AddHelperServices(IServiceCollection services) =>
        services.AddSingleton<IDBConnectionFactory, DBConnectionFactory>().
            AddSingleton<IDapperWrapperService, DapperWrapperService>();

    private static void AddContext(IServiceCollection services) =>
        services.AddSingleton<IContext, Context>();

    private static void AddCors(IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy => {
                    policy.WithOrigins("http://localhost")                
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

    private static void AddMiddleWares(IApplicationBuilder builder)
        => builder.UseMiddleware<RepsonseCorsHeaderMiddleware>()
            .UseMiddleware<EnableRequestBodyBufferingMiddleware>()
            .UseCors();
}