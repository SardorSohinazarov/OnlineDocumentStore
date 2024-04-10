using Microsoft.AspNetCore.Http;

namespace OnlineDocumentStore.Application.Models
{
    public class PDFFile
    {
        public IFormFile File { get; set; }
    }
}
