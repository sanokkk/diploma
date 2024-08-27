using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Models;

#pragma warning disable CS8618
/// <summary>
/// Информация для рассылки оповещений на почту
/// </summary>
[Table("UserInfos")]
public class UserInfo : BaseEntity
{
    public string Email { get; set; }

    public string Name { get; set; }

    public ConditionState NotifyLevel { get; set; }
}