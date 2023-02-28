using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelStateError.Models;

namespace ModelStateError.Data
{
    public class ModelStateErrorContext : DbContext
    {
        public ModelStateErrorContext (DbContextOptions<ModelStateErrorContext> options)
            : base(options)
        {
        }

        public DbSet<ModelStateError.Models.Contact> Contact { get; set; } = default!;
    }
}
