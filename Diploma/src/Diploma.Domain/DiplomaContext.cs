using Diploma.Domain.Models;
using Diploma.Domain.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Diploma.Domain;

public sealed class DiplomaContext : DbContext
{
    public DiplomaContext(DbContextOptions<DiplomaContext> opt) : base(opt) { }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<SensorData> SensorData { get; set; }
    public DbSet<UserInfo> UserInfo { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ParameterTypeWrapper> ParameterTypes { get; set; }
    public DbSet<UnitsReports> UnitsReports { get; set; }
    public DbSet<ErrorType> ErrorTypes { get; set; }
    public DbSet<Error> Errors { get; set; }
    public DbSet<Calculation> Calculations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql("Server=localhost;Database=diplomchik;User Id=admin;Password=admax;Port=5432;Include Error Detail=true;");
        //optionsBuilder.UseNpgsql("Server=postgres;Database=Diploma;User Id=admin;Password=admax;Port=5432;Include Error Detail=true;");
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Unit>()
            .HasMany(x => x.Parameters)
            .WithOne(x => x.Unit);
    }
}