using Microsoft.AspNetCore.Http;

namespace OnlineDocumentStore.Application.Services
{
    public interface IFileService
    {
        ValueTask<string> UploadAsync(IFormFile file, string DiractoryName);
    }
}
