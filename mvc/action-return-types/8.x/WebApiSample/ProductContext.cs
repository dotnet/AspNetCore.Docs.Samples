namespace WebApiSample;

using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Learning ASP.NET Core",
                Description = "A best-selling book covering the fundamentals of ASP.NET Core",
                IsOnSale = true,
            },
            new Product
            {
                Id = 2,
                Name = "Learning EF Core",
                Description = "A best-selling book covering the fundamentals of Entity Framework Core",
                IsOnSale = true,
            },
            new Product
            {
                Id = 3,
                Name = "Learning .NET Standard",
                Description = "A best-selling book covering the fundamentals of .NET Standard",
            },
            new Product
            {
                Id = 4,
                Name = "Learning .NET Core",
                Description = "A best-selling book covering the fundamentals of .NET Core",
            },
            new Product
            {
                Id = 5,
                Name = "Learning C#",
                Description = "A best-selling book covering the fundamentals of C#",
            }
            );

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }
}
