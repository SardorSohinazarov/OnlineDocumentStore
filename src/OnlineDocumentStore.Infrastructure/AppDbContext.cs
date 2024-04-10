using Microsoft.EntityFrameworkCore;
using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
            => Database.Migrate();

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
