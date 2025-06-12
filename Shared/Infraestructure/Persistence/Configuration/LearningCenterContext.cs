using learning_center_back.Tutorials.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_back.Shared.Infrastructure.Persistence.Configuration
{
    public class LearningCenterContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book Entity Configuration
            builder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasAnnotation("CheckConstraint", "LEN(Name) > 0"); // Asegura que no esté vacío

                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasAnnotation("CheckConstraint", "LEN(Description) > 0"); // Asegura que no esté vacío

                entity.HasOne<Category>(c => c.Category);

                entity.HasIndex(c => c.Name)
                    .IsUnique(); // Asegura que sea único
            });


            // Category Entity Configuration
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            });
        }
    }
}