using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Diploma.Domain.Models;

[Table("Units")]
[Index(nameof(Name), IsUnique = true)]
public class Unit : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public UnitType UnitType { get; set; }

    public ICollection<Report> Reports { get; set; }
    
    public ConditionState State { get; set; }
    
    public ICollection<Parameter> Parameters { get; set; }

    public ICollection<SensorData> SensorData { get; set; }
}