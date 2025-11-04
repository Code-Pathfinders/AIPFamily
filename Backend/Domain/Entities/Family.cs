namespace Domain.Entities;

/// <summary>
/// 家族实体
/// </summary>
public class Family : BaseEntity
{
    /// <summary>
    /// 家族名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 家族描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 家族起源地
    /// </summary>
    public string? OriginPlace { get; set; }

    /// <summary>
    /// 家族成员集合
    /// </summary>
    public virtual ICollection<FamilyMember> Members { get; set; } = new List<FamilyMember>();

    /// <summary>
    /// 图形布局配置（JSON格式）
    /// </summary>
    public string? GraphLayout { get; set; }
}
