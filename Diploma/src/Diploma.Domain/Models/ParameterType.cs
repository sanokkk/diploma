using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
public enum ParameterType
{
    [EnumMember(Value = "По умолчанию")] None = 0,
    [EnumMember(Value = "Давления")] Pressure = 1,
    [EnumMember(Value = "Вибрация")] Vibratuion = 2,
    [EnumMember(Value = "Уровень")] Level = 3,
    [EnumMember(Value = "Температура")] Temperature = 4,
    [EnumMember(Value = "Скорость конвейера")] ConveyorSpeed = 5,
}

[Table("ParamterTypes")]
public class ParameterTypeWrapper
{
    public int Id { get; set; }
    public string? ParameterType { get; set; }

    public UnitType UnitType { get; set; }
    public bool IsStatic { get; set; } = false;

    public int Weight { get; set; }
}