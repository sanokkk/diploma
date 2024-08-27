using Diploma.Domain;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Instance.HostedServices;

public class MigrationHostedService : IHostedService
{
    //private readonly DiplomaContext _diplomaContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MigrationHostedService> _logger;

    public MigrationHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<MigrationHostedService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начинаю проверку для миграций");

        try
        {
            var context = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<DiplomaContext>();
            var migrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
            if (migrations.Any())
            {
                _logger.LogInformation("Выполняю миграцию");
                await context.Database.MigrateAsync(cancellationToken);

                return;
            }

            _logger.LogInformation(" Миграции не нужны");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка выполнения миграции: {ex.Message}");
        }
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
