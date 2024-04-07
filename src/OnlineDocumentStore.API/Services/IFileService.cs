namespace OnlineDocumentStore.API.Services
{
    public interface IFileService
    {
        ValueTask<string> UploadAsync(IFormFile file, string DiractoryName);
    }
}
