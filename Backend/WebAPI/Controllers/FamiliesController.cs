using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// 家族管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FamiliesController : ControllerBase
{
    private readonly IFamilyService _familyService;

    public FamiliesController(IFamilyService familyService)
    {
        _familyService = familyService;
    }

    /// <summary>
    /// 获取所有家族
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FamilyDto>>> GetAll()
    {
        var families = await _familyService.GetAllFamiliesAsync();
        return Ok(families);
    }

    /// <summary>
    /// 根据ID获取家族
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FamilyDto>> GetById(Guid id)
    {
        var family = await _familyService.GetFamilyByIdAsync(id);
        if (family == null)
            return NotFound();

        return Ok(family);
    }

    /// <summary>
    /// 创建家族
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<FamilyDto>> Create([FromBody] CreateFamilyDto dto)
    {
        var family = await _familyService.CreateFamilyAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = family.Id }, family);
    }

    /// <summary>
    /// 更新家族
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<FamilyDto>> Update(Guid id, [FromBody] UpdateFamilyDto dto)
    {
        var family = await _familyService.UpdateFamilyAsync(id, dto);
        if (family == null)
            return NotFound();

        return Ok(family);
    }

    /// <summary>
    /// 删除家族
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _familyService.DeleteFamilyAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
