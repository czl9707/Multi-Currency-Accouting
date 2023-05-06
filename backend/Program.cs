using Accountant.Models;
using Accountant.Services.DB;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        AddServices(builder.Services);
        
        var app = builder.Build();

        // app.UseHttpsRedirection();
        app.Use(async (context, next) =>{
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            await next.Invoke();
        });

        app.UseCors();
        app.MapControllers();
        app.Run();
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
                    policy.WithOrigins("http://localhost");
                });
        });
}