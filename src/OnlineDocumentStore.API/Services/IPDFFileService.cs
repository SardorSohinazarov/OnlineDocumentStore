using OnlineDocumentStore.API.Models;

namespace OnlineDocumentStore.API.Services
{
    public interface IPDFFileService
    {
        ValueTask<string> AddPhoto(PDFFile pdfFile);
    }
}
