using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Diploma.Domain.Models;

[Table(nameof(Calculation))]
public class Calculation
{
    public Guid Id { get; set; }

    public Unit Unit { get; set; }

    public int? StaticIndex { get; set; }

    public int? DynamicIndex { get; set; }

    public DateTimeOffset WhenCalculated { get; set; }
}
