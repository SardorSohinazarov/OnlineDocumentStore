using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineDocumentStore.Application.Abstractions;
using OnlineDocumentStore.Application.Abstractions.Repositories;
using OnlineDocumentStore.Infrastructure.Repositories;

namespace OnlineDocumentStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddDbContext<AppDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
