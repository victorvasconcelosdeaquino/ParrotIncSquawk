using Microsoft.EntityFrameworkCore;
using ParrotIncSquawk.Entities;

namespace ParrotIncSquawk.Persistence
{
    public class SquawkContext : DbContext
    {
        public SquawkContext(DbContextOptions<SquawkContext> options)
            : base(options)
        { }

        public DbSet<Squawk>? Squawks { get; set; }
    }
}
