using Microsoft.AspNetCore.Http;

namespace OnlineDocumentStore.Application.DataTransferObjects
{
    public class PDFFile
    {
        public IFormFile File { get; set; }
    }
}
