using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineDocumentStore.Application.Services.AuthServices;
using OnlineDocumentStore.Application.Services.FileServices;
using OnlineDocumentStore.Application.Services.Halpers;
using OnlineDocumentStore.Application.Services.JWTServices;
using OnlineDocumentStore.Application.Services.PDFFileServices;
using OnlineDocumentStore.Application.Services.QRCodeServices;

namespace OnlineDocumentStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<IPDFFileService, PDFFileService>()
                .AddScoped<IQRCodeService, QRCodeService>()
                .AddScoped<IJWTService, JWTService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
