﻿using Microsoft.EntityFrameworkCore;
using OnlineDocumentStore.Application.Abstractions;
using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
            => Database.Migrate();

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        async ValueTask<int> IAppDbContext.SaveChangesAsync(CancellationToken cancellationToken)
            => await base.SaveChangesAsync(cancellationToken);
    }
}
