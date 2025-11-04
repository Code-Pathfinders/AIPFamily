namespace Application.DTOs;

/// <summary>
/// 家族数据传输对象
/// </summary>
public class FamilyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OriginPlace { get; set; }
    public string? GraphLayout { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 创建家族请求
/// </summary>
public class CreateFamilyDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OriginPlace { get; set; }
}

/// <summary>
/// 更新家族请求
/// </summary>
public class UpdateFamilyDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OriginPlace { get; set; }
    public string? GraphLayout { get; set; }
}
