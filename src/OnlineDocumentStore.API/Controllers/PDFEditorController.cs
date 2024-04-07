using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.API.Models;
using OnlineDocumentStore.API.Services;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PDFEditorController : ControllerBase
    {
        private readonly IPDFFileService _pdfFileService;

        public PDFEditorController(IPDFFileService pdfFileService)
            => _pdfFileService = pdfFileService;

        [HttpPost]
        public async Task<IActionResult> AddQRCodeAsync([FromForm] PDFFile pdfFile)
        {
            var editedFile = await _pdfFileService.AddPhotoAsync(pdfFile);

            return Ok(editedFile);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileAsync()
        {
            return Ok();
        }
    }
}
