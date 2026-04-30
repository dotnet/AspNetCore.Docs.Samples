using App.Models;

namespace App.Data;

public static class AppDbSeeder
{
    public static async Task Seed(WebApplication app)
    {
        // Create and seed the database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDb>();
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return;
            }

            Customer[] customers = {
                new Customer
                {
                    Id = "1",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@gmail.com",
                    PhoneNumbers = [new() {Number = "123-456-7890", Type = PhoneNumberType.Mobile}],
                    Address = new Address
                    {
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "TX"
                    }
                },
                new()
                {
                    Id = "2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumbers = [new() {Number = "555-987-6543", Type = PhoneNumberType.Mobile}],
                    Address = new Address
                    {
                        Street = "456 Oak Ave",
                        City = "Somewhere",
                        State = "USA"
                    }
                },
                new()
                {
                    Id = "3",
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    PhoneNumbers = [new() {Number = "555-555-5555", Type = PhoneNumberType.Mobile}],
                    Address = new Address
                    {
                        Street = "789 Pine Rd",
                        City = "Elsewhere",
                        State = "USA"
                    }
                }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }
}