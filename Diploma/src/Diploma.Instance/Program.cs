using Diploma.Domain.Configuration.DI;
using Diploma.Instance.Configuration.DI;
using Diploma.Instance.HostedServices;
using Diploma.Instance.Hubs;
using Diploma.Logic.Configuration.DI;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDomainDependencies();
builder.Services.ConfigureInstanceDependencies();
builder.Services.ConfigureLogic();
builder.Services.ConfigureTelemetry();
builder.Services.AddHostedService<MigrationHostedService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

//InstanceDependency.InitialMigrate(app);

app.UseCors("react");

app.UseMetricServer();
app.MapPrometheusScrapingEndpoint();

app.UseHttpMetrics();

app.UseRouting();
app.MapControllers();
app.MapMetrics();
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
//app.UseHttpsRedirection();

app.MapHub<IndexHub>("/index");
app.MapHub<PlcHub>("/plc");

app.Run();