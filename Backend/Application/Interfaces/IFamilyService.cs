using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// 家族服务接口
/// </summary>
public interface IFamilyService
{
    /// <summary>
    /// 获取所有家族
    /// </summary>
    Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync();

    /// <summary>
    /// 根据ID获取家族
    /// </summary>
    Task<FamilyDto?> GetFamilyByIdAsync(Guid id);

    /// <summary>
    /// 创建家族
    /// </summary>
    Task<FamilyDto> CreateFamilyAsync(CreateFamilyDto dto);

    /// <summary>
    /// 更新家族
    /// </summary>
    Task<FamilyDto?> UpdateFamilyAsync(Guid id, UpdateFamilyDto dto);

    /// <summary>
    /// 删除家族
    /// </summary>
    Task<bool> DeleteFamilyAsync(Guid id);
}
