using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models.ManyToMany;

[Table("Узлы_Отчеты")]
public class UnitsReports
{
    [Key]
    [Column("Идентификатор")]
    public Guid Id { get; set; }

    [ForeignKey("unit_id")]
    public Unit Unit { get; set; }

    [ForeignKey("Идентификатор_Отчета")]
    public Report Report { get; set; }
}
