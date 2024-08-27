using Diploma.Domain.Repositories.ParameterType;
using Diploma.Domain.Repositories.Units;
using Diploma.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Domain.Configuration.DI;

public static class DomainDependency
{
    public static void ConfigureDomainDependencies(this IServiceCollection services)
    {
        services.AddDbContext<DiplomaContext>(opt => opt.UseNpgsql("Server=localhost;Database=Diplomm;User Id=admin;Password=admax;Port=5432;Include Error Detail=true;"));
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IParameterTypeRepository, ParameterTypeRepository>();
    }
}