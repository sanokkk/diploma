using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618

[Table("SensorDatas")]
public class SensorData
{
    public Guid Id { get; set; }

    [ForeignKey("unit_id")]
    public Unit Node { get; set; }

    public double Value { get; set; }

    public DateTimeOffset Happened { get; set; }

    [ForeignKey("parameter_type_id")]
    public ParameterTypeWrapper ParameterType { get; set; }
    
}