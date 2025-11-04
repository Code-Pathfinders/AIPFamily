using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// 家族成员管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FamilyMembersController : ControllerBase
{
    private readonly IFamilyMemberService _memberService;

    public FamilyMembersController(IFamilyMemberService memberService)
    {
        _memberService = memberService;
    }

    /// <summary>
    /// 获取指定家族的所有成员
    /// </summary>
    [HttpGet("family/{familyId}")]
    public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> GetByFamilyId(Guid familyId)
    {
        var members = await _memberService.GetMembersByFamilyIdAsync(familyId);
        return Ok(members);
    }

    /// <summary>
    /// 根据ID获取成员
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<FamilyMemberDto>> GetById(Guid id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    /// <summary>
    /// 创建家族成员
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<FamilyMemberDto>> Create([FromBody] CreateFamilyMemberDto dto)
    {
        var member = await _memberService.CreateMemberAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    /// <summary>
    /// 更新家族成员
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<FamilyMemberDto>> Update(Guid id, [FromBody] UpdateFamilyMemberDto dto)
    {
        var member = await _memberService.UpdateMemberAsync(id, dto);
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    /// <summary>
    /// 删除家族成员
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _memberService.DeleteMemberAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// 批量更新成员位置
    /// </summary>
    [HttpPost("positions")]
    public async Task<ActionResult> UpdatePositions([FromBody] List<MemberPositionDto> positions)
    {
        var positionList = positions.Select(p => (p.Id, p.X, p.Y)).ToList();
        await _memberService.UpdateMembersPositionAsync(positionList);
        return Ok();
    }
}

/// <summary>
/// 成员位置DTO
/// </summary>
public class MemberPositionDto
{
    public Guid Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}
