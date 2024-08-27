using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
public enum UnitType {
    /// <summary>
    /// Экструдер
    /// </summary>
    [EnumMember(Value = "Экструдер")] [XmlEnum("0")]Экструдер = 0,
    /// <summary>
    /// Конвейер
    /// </summary>
    [EnumMember(Value = "Конвейер")] [XmlEnum("1")]Конвеер = 1,
    /// <summary>
    /// Формующая установка
    /// </summary>
    [EnumMember(Value = "Формующая установка")][XmlEnum("2")]ФормующаяУстановка = 2,
    /// <summary>
    /// Охлаждающая установка
    /// </summary>
    [EnumMember(Value = "Охлаждающая установка")][XmlEnum("3")]РезательноеОборудование = 3,
}