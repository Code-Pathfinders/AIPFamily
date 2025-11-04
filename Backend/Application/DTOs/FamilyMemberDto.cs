namespace Application.DTOs;

/// <summary>
/// 家族成员数据传输对象
/// </summary>
public class FamilyMemberDto
{
    public Guid Id { get; set; }
    public Guid FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? FatherId { get; set; }
    public Guid? MotherId { get; set; }
    public Guid? SpouseId { get; set; }
    public int Generation { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CustomAttributes { get; set; }
    public string? NodeStyle { get; set; }
    public string? Notes { get; set; }
    public double? PositionX { get; set; }
    public double? PositionY { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 创建家族成员请求
/// </summary>
public class CreateFamilyMemberDto
{
    public Guid FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? FatherId { get; set; }
    public Guid? MotherId { get; set; }
    public Guid? SpouseId { get; set; }
    public int Generation { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CustomAttributes { get; set; }
    public string? NodeStyle { get; set; }
    public string? Notes { get; set; }
    public double? PositionX { get; set; }
    public double? PositionY { get; set; }
}

/// <summary>
/// 更新家族成员请求
/// </summary>
public class UpdateFamilyMemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? FatherId { get; set; }
    public Guid? MotherId { get; set; }
    public Guid? SpouseId { get; set; }
    public int Generation { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CustomAttributes { get; set; }
    public string? NodeStyle { get; set; }
    public string? Notes { get; set; }
    public double? PositionX { get; set; }
    public double? PositionY { get; set; }
}
