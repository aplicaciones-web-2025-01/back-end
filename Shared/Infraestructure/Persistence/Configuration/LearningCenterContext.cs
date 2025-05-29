using learning_center_back.Security.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_center_back.Shared.Infraestructure.Persistence.Configuration;

public class LearningCenterContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Tutorial> Tutorials { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tutorial>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).IsRequired();
        });

        builder.Entity<Category>(entity =>
        {
            entity.HasOne(c => c.Tutorial)
                .WithMany(t => t.Categories)
                .HasForeignKey(c => c.TutorialId);
        });
    }
}