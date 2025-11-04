namespace Domain.Entities;

/// <summary>
/// 家族成员实体
/// </summary>
public class FamilyMember : BaseEntity
{
    /// <summary>
    /// 所属家族ID
    /// </summary>
    public Guid FamilyId { get; set; }

    /// <summary>
    /// 所属家族
    /// </summary>
    public virtual Family Family { get; set; } = null!;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 性别（Male/Female）
    /// </summary>
    public string Gender { get; set; } = "Male";

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 去世日期
    /// </summary>
    public DateTime? DeathDate { get; set; }

    /// <summary>
    /// 父亲ID
    /// </summary>
    public Guid? FatherId { get; set; }

    /// <summary>
    /// 母亲ID
    /// </summary>
    public Guid? MotherId { get; set; }

    /// <summary>
    /// 配偶ID
    /// </summary>
    public Guid? SpouseId { get; set; }

    /// <summary>
    /// 世代（第几代）
    /// </summary>
    public int Generation { get; set; }

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 自定义属性（JSON格式）
    /// </summary>
    public string? CustomAttributes { get; set; }

    /// <summary>
    /// 节点样式配置（JSON格式，包含颜色等）
    /// </summary>
    public string? NodeStyle { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// X坐标（用于保存图形位置）
    /// </summary>
    public double? PositionX { get; set; }

    /// <summary>
    /// Y坐标（用于保存图形位置）
    /// </summary>
    public double? PositionY { get; set; }
}
