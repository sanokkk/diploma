using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
/// <summary>
/// Параметры для определенного датчика (например, датчик температуры будет ассоциироваться с ParameterType = 4,
/// minValue = XXX, maxValue = YYY)
/// </summary>
[Table("Parameters")]
public class Parameter : BaseEntity
{
    [ForeignKey("unit_id")]
    public Unit Unit { get; set; }

    public int ParameterTypeId { get; set; }
    public ParameterTypeWrapper ParameterType { get; set; }

    public double MaxValue { get; set; } = 1;

    public double MinValue { get; set; } = 1;
}