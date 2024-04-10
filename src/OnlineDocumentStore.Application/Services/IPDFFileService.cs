using OnlineDocumentStore.Application.Models;

namespace OnlineDocumentStore.Application.Services
{
    public interface IPDFFileService
    {
        ValueTask<string> AddPhotoAsync(PDFFile pdfFile);
        ValueTask<string> AddPhotoAsync(PDFFile pdfFile, double? x, double? y, double? length);
    }
}
