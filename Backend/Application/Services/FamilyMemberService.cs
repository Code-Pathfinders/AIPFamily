using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// 家族成员服务实现
/// </summary>
public class FamilyMemberService : IFamilyMemberService
{
    private readonly FamilyDbContext _context;

    public FamilyMemberService(FamilyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FamilyMemberDto>> GetMembersByFamilyIdAsync(Guid familyId)
    {
        var members = await _context.FamilyMembers
            .Where(m => m.FamilyId == familyId)
            .OrderBy(m => m.Generation)
            .ThenBy(m => m.Name)
            .ToListAsync();

        return members.Select(MapToDto);
    }

    public async Task<FamilyMemberDto?> GetMemberByIdAsync(Guid id)
    {
        var member = await _context.FamilyMembers.FindAsync(id);
        return member == null ? null : MapToDto(member);
    }

    public async Task<FamilyMemberDto> CreateMemberAsync(CreateFamilyMemberDto dto)
    {
        var member = new FamilyMember
        {
            FamilyId = dto.FamilyId,
            Name = dto.Name,
            Gender = dto.Gender,
            BirthDate = dto.BirthDate,
            DeathDate = dto.DeathDate,
            FatherId = dto.FatherId,
            MotherId = dto.MotherId,
            SpouseId = dto.SpouseId,
            Generation = dto.Generation,
            AvatarUrl = dto.AvatarUrl,
            CustomAttributes = dto.CustomAttributes,
            NodeStyle = dto.NodeStyle,
            Notes = dto.Notes,
            PositionX = dto.PositionX,
            PositionY = dto.PositionY
        };

        _context.FamilyMembers.Add(member);
        await _context.SaveChangesAsync();

        return MapToDto(member);
    }

    public async Task<FamilyMemberDto?> UpdateMemberAsync(Guid id, UpdateFamilyMemberDto dto)
    {
        var member = await _context.FamilyMembers.FindAsync(id);
        if (member == null) return null;

        member.Name = dto.Name;
        member.Gender = dto.Gender;
        member.BirthDate = dto.BirthDate;
        member.DeathDate = dto.DeathDate;
        member.FatherId = dto.FatherId;
        member.MotherId = dto.MotherId;
        member.SpouseId = dto.SpouseId;
        member.Generation = dto.Generation;
        member.AvatarUrl = dto.AvatarUrl;
        member.CustomAttributes = dto.CustomAttributes;
        member.NodeStyle = dto.NodeStyle;
        member.Notes = dto.Notes;
        member.PositionX = dto.PositionX;
        member.PositionY = dto.PositionY;
        member.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(member);
    }

    public async Task<bool> DeleteMemberAsync(Guid id)
    {
        var member = await _context.FamilyMembers.FindAsync(id);
        if (member == null) return false;

        _context.FamilyMembers.Remove(member);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateMembersPositionAsync(List<(Guid Id, double X, double Y)> positions)
    {
        foreach (var (id, x, y) in positions)
        {
            var member = await _context.FamilyMembers.FindAsync(id);
            if (member != null)
            {
                member.PositionX = x;
                member.PositionY = y;
                member.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private static FamilyMemberDto MapToDto(FamilyMember member)
    {
        return new FamilyMemberDto
        {
            Id = member.Id,
            FamilyId = member.FamilyId,
            Name = member.Name,
            Gender = member.Gender,
            BirthDate = member.BirthDate,
            DeathDate = member.DeathDate,
            FatherId = member.FatherId,
            MotherId = member.MotherId,
            SpouseId = member.SpouseId,
            Generation = member.Generation,
            AvatarUrl = member.AvatarUrl,
            CustomAttributes = member.CustomAttributes,
            NodeStyle = member.NodeStyle,
            Notes = member.Notes,
            PositionX = member.PositionX,
            PositionY = member.PositionY,
            CreatedAt = member.CreatedAt,
            UpdatedAt = member.UpdatedAt
        };
    }
}
