using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Diploma.Domain.Models;

[Table("Reports")]
public class Report
{
    public Guid Id { get; set; }

    public Unit[] Units { get; set; }

    public DateTimeOffset WhenCreated { get; set; }

    public string ContentPath { get; set; }
}
