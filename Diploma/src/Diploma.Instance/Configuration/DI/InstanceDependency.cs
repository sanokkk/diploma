using System.Diagnostics.Metrics;
using Diploma.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Prometheus;

namespace Diploma.Instance.Configuration.DI;

public static class InstanceDependency
{
    private const string SerivceName = "Diploma";
    public static void ConfigureInstanceDependencies(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSignalR(x =>
        {
            x.KeepAliveInterval = TimeSpan.FromMinutes(5);
            x.MaximumReceiveMessageSize = 102400000;
            x.EnableDetailedErrors = true;
        });
        services.AddSignalRCore();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.AddSignalRSwaggerGen();
        });
        
        services.AddCors(opt =>
        {
            opt.AddPolicy("react", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .AllowAnyMethod();

            });
        });
    }

    public static void ConfigureTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(opt => opt.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(SerivceName))
                                   .AddPrometheusExporter());
    }

    public static void InitialMigrate(WebApplication app)
    {
        try
        {
            var context = app.Services.GetService<DiplomaContext>();

            if (!context!.Database.CanConnect()) return;

            var isMigrationNeeded = context.Database.GetPendingMigrations().Any();
            if (isMigrationNeeded)
            {
                context.Database.Migrate();
                Console.WriteLine("Выполняю миграцию");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при миграции: ", ex.Message);
        }
    }
}