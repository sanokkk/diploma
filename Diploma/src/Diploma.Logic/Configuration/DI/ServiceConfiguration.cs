using Diploma.Logic.Metrics;
using Diploma.Logic.Services.Implementations;
using Diploma.Logic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Logic.Configuration.DI;

public static class ServiceConfiguration
{
    public static void ConfigureLogic(this IServiceCollection services)
    {
        services.AddScoped<IConditionService, ConditionService>();
        services.AddScoped<INotifyService, EmailNotificationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddMemoryCache();
        //services.AddSingleton<IMetrics, DiagnosticMetrics>();
        services.AddSingleton<MetricsCollector>();
    }
}