using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

/// <summary>
/// 家族服务实现
/// </summary>
public class FamilyService : IFamilyService
{
    private readonly FamilyDbContext _context;

    public FamilyService(FamilyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync()
    {
        var families = await _context.Families
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return families.Select(MapToDto);
    }

    public async Task<FamilyDto?> GetFamilyByIdAsync(Guid id)
    {
        var family = await _context.Families.FindAsync(id);
        return family == null ? null : MapToDto(family);
    }

    public async Task<FamilyDto> CreateFamilyAsync(CreateFamilyDto dto)
    {
        var family = new Family
        {
            Name = dto.Name,
            Description = dto.Description,
            OriginPlace = dto.OriginPlace
        };

        _context.Families.Add(family);
        await _context.SaveChangesAsync();

        return MapToDto(family);
    }

    public async Task<FamilyDto?> UpdateFamilyAsync(Guid id, UpdateFamilyDto dto)
    {
        var family = await _context.Families.FindAsync(id);
        if (family == null) return null;

        family.Name = dto.Name;
        family.Description = dto.Description;
        family.OriginPlace = dto.OriginPlace;
        family.GraphLayout = dto.GraphLayout;
        family.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(family);
    }

    public async Task<bool> DeleteFamilyAsync(Guid id)
    {
        var family = await _context.Families.FindAsync(id);
        if (family == null) return false;

        _context.Families.Remove(family);
        await _context.SaveChangesAsync();

        return true;
    }

    private static FamilyDto MapToDto(Family family)
    {
        return new FamilyDto
        {
            Id = family.Id,
            Name = family.Name,
            Description = family.Description,
            OriginPlace = family.OriginPlace,
            GraphLayout = family.GraphLayout,
            CreatedAt = family.CreatedAt,
            UpdatedAt = family.UpdatedAt
        };
    }
}
