using System.Runtime.Serialization;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
public enum ConditionState
{
    [EnumMember(Value = "Идеальное")]Идеальное = 1,
    [EnumMember(Value = "Хорошое")]Хорошее = 2,
    [EnumMember(Value = "Нормальное")]Нормальное = 3,
    [EnumMember(Value = "Плохое")]Плохое = 4,
    [EnumMember(Value = "Чрезвычайное")]Чрезвычайное = 5,
}