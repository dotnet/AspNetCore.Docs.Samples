using Microsoft.EntityFrameworkCore;

namespace Contacts.Data;

public class ModelStateErrorContext : DbContext
{
    public ModelStateErrorContext(DbContextOptions<ModelStateErrorContext> options)
        : base(options)
    {
    }

    public DbSet<Contacts.Models.Contact> Contact { get; set; } = default!;
}
