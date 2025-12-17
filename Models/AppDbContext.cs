using Microsoft.EntityFrameworkCore;

namespace GraphQL_Learning.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // agregar DbSet
    }
}
