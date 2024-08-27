using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Diploma.Domain.Models;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Diploma.DTO.Controllers.UserController.Requests;

public sealed record CreateUnitDto(
    [Required]UnitType Type,
    [Required]string Name,
    [Required, MinLength(3)]CreateParameterDto[] Parameters);


public sealed record CreateUnitFromFileDto
{
    public CreateUnitFromFileDto() { }

    public CreateUnitFromFileDto(UnitType unitType, string name, CreateParameterFromFileDto[] parameters)
    {
        UnitType = unitType;
        Name = name;
        Parameters = parameters;
    }

    [Required, XmlElement("УзелТип")]
    public UnitType UnitType { get; set; }

    [Required, XmlElement("Имя")]
    public string Name { get; set; }

    [Required, MinLength(3), XmlElement("Параметр")]
    public CreateParameterFromFileDto[] Parameters { get; set; }
}

[XmlRoot("Узлы")]
public sealed record CreateUnitsFromFileDto
{
    [XmlElement("Узел")]
    public CreateUnitFromFileDto[] Units { get; set; }
}
