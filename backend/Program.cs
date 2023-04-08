using Accountant.Models;
using Accountant.Services.DB;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        InjectServices(builder.Services);
        
        var app = builder.Build();

        var c = app.Services.GetRequiredService<IContext>();

        // app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }

    private static void InjectServices(IServiceCollection services){
        InjectHelperServices(services);
        InjectContext(services);
    }

    private static void InjectHelperServices(IServiceCollection services) =>
        services.AddSingleton<IDBConnectionFactory, DBConnectionFactory>().
            AddSingleton<IDapperWrapperService, DapperWrapperService>();

    private static void InjectContext(IServiceCollection services) =>
        services.AddSingleton<IContext, Context>();
}