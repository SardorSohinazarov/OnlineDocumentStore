using Microsoft.Extensions.DependencyInjection;
using OnlineDocumentStore.Application.Services;

namespace OnlineDocumentStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddScoped<IFileService, FileService>()
                .AddScoped<IPDFFileService, PDFFileService>()
                .AddScoped<IQRCodeService, QRCodeService>();

            return services;
        }
    }
}
