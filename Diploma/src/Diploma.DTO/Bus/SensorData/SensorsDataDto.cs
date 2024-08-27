#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Diploma.DTO.Bus.SensorData;

public sealed record SensorsDataDto
{
    public IReadOnlyCollection<Domain.Models.SensorData> Body { get; set; }
}
