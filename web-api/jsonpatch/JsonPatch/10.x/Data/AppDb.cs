using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Data;

public class AppDb : DbContext
{
    public required DbSet<Customer> Customers { get; set; }

    public AppDb(DbContextOptions<AppDb> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships here if needed
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        // Configure Address as an owned type (stored as JSON in SQLite)
        modelBuilder.Entity<Customer>()
            .OwnsOne(c => c.Address, address =>
            {
                address.ToJson();
            });

        // Configure PhoneNumbers as an owned collection (stored as JSON in SQLite)
        modelBuilder.Entity<Customer>()
            .OwnsMany(c => c.PhoneNumbers, phoneNumber =>
            {
                phoneNumber.ToJson();
            });
    }
}