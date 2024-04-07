namespace OnlineDocumentStore.API.Services
{
    public interface IFileService
    {
        ValueTask<string> Upload(IFormFile file, string DiractoryName);
    }
}
