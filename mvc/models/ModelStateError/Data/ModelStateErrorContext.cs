using Microsoft.EntityFrameworkCore;

namespace ModelStateError.Data;

public class ModelStateErrorContext : DbContext
{
    public ModelStateErrorContext(DbContextOptions<ModelStateErrorContext> options)
        : base(options)
    {
    }

    public DbSet<ModelStateError.Models.Contact> Contact { get; set; } = default!;
}
