using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IResultImplementation.Models;
using Microsoft.EntityFrameworkCore;

namespace IResultImplementation.Data
{
    public class IResultImplementationContext : DbContext
    {
        public IResultImplementationContext(DbContextOptions<IResultImplementationContext> options)
            : base(options)
        {
        }
        public IResultImplementationContext()
        {
        }


        public virtual DbSet<Contact> Contact { get; set; } = default!;
    }
}
