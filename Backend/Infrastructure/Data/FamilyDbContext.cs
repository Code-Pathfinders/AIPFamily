using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// 家族数据库上下文
/// </summary>
public class FamilyDbContext : DbContext
{
    public FamilyDbContext(DbContextOptions<FamilyDbContext> options) : base(options)
    {
    }

    public DbSet<Family> Families { get; set; }
    public DbSet<FamilyMember> FamilyMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置Family实体
        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.OriginPlace).HasMaxLength(200);
            
            // 配置一对多关系
            entity.HasMany(e => e.Members)
                .WithOne(m => m.Family)
                .HasForeignKey(m => m.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置FamilyMember实体
        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            
            // 创建索引
            entity.HasIndex(e => e.FamilyId);
            entity.HasIndex(e => e.Generation);
        });
    }
}
