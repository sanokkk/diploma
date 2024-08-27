using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using System.Xml.Serialization;
using Diploma.Domain.Models;

namespace Diploma.DTO.Controllers.UserController.Requests;

//[XmlRoot("ТипПараметр")]
public sealed record AddParameterTypeDto
{
    public AddParameterTypeDto()
    {
        
    }
    
    public AddParameterTypeDto(string name, int weight, UnitType unitType, bool isStatic)
    {
        Name = name;
        Weight = weight;
        UnitType = unitType;
        IsStatic = isStatic;
    }

    [Required] [XmlElement("Имя")] public string Name { get; set; }

    [Required, Range(1, 10)]
    [XmlElement("Вес")]
    public int Weight { get; set; }

    [Required] [XmlElement("ТипУзла")][Range(0, 3)] public UnitType UnitType { get; set; }
    [XmlElement("Статический")] public bool IsStatic { get; set; } = false;
}

[XmlRoot("Типы")]
public sealed record CreateParameterTypesDto
{
    [XmlElement("ТипПараметр")]
    public AddParameterTypeDto[] Parameters { get; set; }
}
    