using FluentValidation;

namespace Diploma.DTO.Bus.SensorData;

public class SensorsDataValidator : AbstractValidator<SensorsDataDto>
{
    public SensorsDataValidator()
    {
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.Body).ForEach(x => x.NotNull());
    }
}