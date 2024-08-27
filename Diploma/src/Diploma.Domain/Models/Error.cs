using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

[Table("Errors")]

public class Error
{
    public Guid Id { get; set; }

    [ForeignKey("error_type")]
    public ErrorType ErrorType { get; init; }

    public string Description { get; set; }

    public DateTimeOffset WhenHappened { get; set; }
}

public class ErrorType
{
    public int Id { get; set; }

    public ErrorTypeEnum Type { get; set; }

    public string Name { get; set; }
}

public enum ErrorTypeEnum
{
    StaticParameterDidntCome = 1,
    StaticParameterLimit = 2,
    DynamicParameterLimit = 3,
}