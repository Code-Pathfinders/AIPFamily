namespace Domain.Entities;

/// <summary>
/// 基础实体类
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// 实体唯一标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
