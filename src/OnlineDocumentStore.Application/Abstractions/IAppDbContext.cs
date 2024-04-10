using Microsoft.EntityFrameworkCore;
using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Application.Abstractions
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; set; }

        ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
