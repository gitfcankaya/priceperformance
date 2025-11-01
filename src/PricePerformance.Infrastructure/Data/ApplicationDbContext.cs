using Microsoft.EntityFrameworkCore;
using PricePerformance.Core.Entities;

namespace PricePerformance.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Country configuration
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Platform configuration
        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BaseUrl).IsRequired().HasMaxLength(500);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Slug).IsUnique();
            
            entity.HasOne(e => e.ParentCategory)
                .WithMany(e => e.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(5000);
            entity.HasIndex(e => e.Slug);
            
            entity.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ProductPrice configuration
        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ProductUrl).HasMaxLength(1000);
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductPrices)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Platform)
                .WithMany(e => e.ProductPrices)
                .HasForeignKey(e => e.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Country)
                .WithMany(e => e.ProductPrices)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasIndex(e => new { e.ProductId, e.PlatformId, e.CountryId, e.PriceDate });
        });

        // ProductReview configuration
        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ReviewerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ReviewText).IsRequired().HasMaxLength(5000);
            entity.Property(e => e.Rating).IsRequired();
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductSpecification configuration
        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SpecificationKey).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SpecificationValue).IsRequired().HasMaxLength(500);
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.Specifications)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
