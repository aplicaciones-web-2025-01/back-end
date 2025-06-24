using learning_center_back.Security.Domai_.Entities;
using learning_center_back.Tutorials.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_back.Shared.Infrastructure.Persistence.Configuration
{
    public class LearningCenterContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

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
                    .HasMaxLength(20);
                
                entity.Property(c => c.PublishDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.CreatedDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(c => c.ModifiedDate)
                    .HasColumnType("DATETIME");

                entity.HasIndex(c => c.Name)
                    .IsUnique();

                entity.HasOne(c => c.Category)
                    .WithMany()
                    .HasForeignKey(c => c.CategoryId);
                
                // Relación corregida con Category
                entity.Property(c => c.CategoryId) // Asegurar que CategoryId esté mapeado
                    .IsRequired();

                entity.HasOne(c => c.Category)
                    .WithMany()
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade); // Definir comportamiento de eliminación
            });

            // Category Entity Configuration
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.CreatedDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(c => c.ModifiedDate)
                    .HasColumnType("DATETIME");
            });

            // Chapter Entity Configuration
            builder.Entity<Chapter>(entity =>
            {
                entity.ToTable("Chapters");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CreatedDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(c => c.ModifiedDate)
                    .HasColumnType("DATETIME");
            });
            
            // Chapter Entity Configuration
            builder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CreatedDate)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(c => c.ModifiedDate)
                    .HasColumnType("DATETIME");
            });
        }
    }
}
