namespace WebApiSample;

using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
