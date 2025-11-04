using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// 家族成员服务接口
/// </summary>
public interface IFamilyMemberService
{
    /// <summary>
    /// 获取指定家族的所有成员
    /// </summary>
    Task<IEnumerable<FamilyMemberDto>> GetMembersByFamilyIdAsync(Guid familyId);

    /// <summary>
    /// 根据ID获取成员
    /// </summary>
    Task<FamilyMemberDto?> GetMemberByIdAsync(Guid id);

    /// <summary>
    /// 创建家族成员
    /// </summary>
    Task<FamilyMemberDto> CreateMemberAsync(CreateFamilyMemberDto dto);

    /// <summary>
    /// 更新家族成员
    /// </summary>
    Task<FamilyMemberDto?> UpdateMemberAsync(Guid id, UpdateFamilyMemberDto dto);

    /// <summary>
    /// 删除家族成员
    /// </summary>
    Task<bool> DeleteMemberAsync(Guid id);

    /// <summary>
    /// 批量更新成员位置
    /// </summary>
    Task<bool> UpdateMembersPositionAsync(List<(Guid Id, double X, double Y)> positions);
}
