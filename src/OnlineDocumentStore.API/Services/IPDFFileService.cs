using OnlineDocumentStore.API.Models;

namespace OnlineDocumentStore.API.Services
{
    public interface IPDFFileService
    {
        ValueTask<string> AddPhotoAsync(PDFFile pdfFile);
        ValueTask<string> AddPhotoAsync(PDFFile pdfFile, double? x, double? y, double? length);
    }
}
