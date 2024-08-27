using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Diploma.Domain.Models;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Diploma.DTO.Controllers.UserController.Requests;

public sealed record CreateParameterDto(
    [Required] int ParameterTypeId, 
    [Required]double MinValue,
    [Required]double MaxValue);

public sealed record CreateParameterFromFileDto
{
    public CreateParameterFromFileDto(string parameterName, double minValue, double maxValue)
    {
        ParameterName = parameterName;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public CreateParameterFromFileDto() { }

    [Required, XmlElement("ПараметрИмя")] public string ParameterName { get; set; }
    [Required, XmlElement("Мин")] public double MinValue { get; set; }
    [Required, XmlElement("Макс")] public double MaxValue { get; set; }
}