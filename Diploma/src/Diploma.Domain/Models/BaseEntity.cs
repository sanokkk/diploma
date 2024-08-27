using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
public abstract class BaseEntity
{
    [Key]
    [Column("Идентификатор")]
    public Guid Id { get; set; }

    [Column("Время создания")]
    public DateTimeOffset CreateAt { get; set; }
    
    [Column("Время обновления")]
    public DateTimeOffset UpdatedAt { get; set; }
}