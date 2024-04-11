using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.Services.DocumentServices;
using OnlineDocumentStore.Application.Services.PDFFileServices;

namespace OnlineDocumentStore.MVC.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IPDFFileService _pdfFileService;

        public DocumentsController(
            IDocumentService documentService,
            IPDFFileService pdfFileService)
        {
            _documentService = documentService;
            _pdfFileService = pdfFileService;
        }

        [Route("/Documents/{id}")]
        public async Task<IActionResult> GetDocumentAsync(Guid id)
        {
            var document = await _documentService.GetByIdAsync(id);
            return View(document);
        }

        public async Task<IActionResult> DownloadFileAsync(string path)
        {
            if (!System.IO.File.Exists(path))
                throw new Exception("File not found");

            var fileInfo = new System.IO.FileInfo(path);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(path), "application/pdf", fileInfo.Name);
        }
    }
}
