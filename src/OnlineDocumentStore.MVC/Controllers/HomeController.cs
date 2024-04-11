using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.DataTransferObjects;
using OnlineDocumentStore.Application.Services.DocumentServices;
using OnlineDocumentStore.Application.Services.PDFFileServices;
using OnlineDocumentStore.MVC.Models;
using System.Diagnostics;

namespace OnlineDocumentStore.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IPDFFileService _pdfFileService;

        public HomeController(IDocumentService documentService, IPDFFileService pdfFileService)
        {
            _documentService = documentService;
            _pdfFileService = pdfFileService;
        }

        [Authorize]
        public async Task<IActionResult> UploadDocumentAsync(PDFFile pdfFile)
        {
            var path = await _pdfFileService.AddPhotoAsync(pdfFile);

            if (!System.IO.File.Exists(path))
                throw new Exception("File not found");

            var fileInfo = new System.IO.FileInfo(path);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(path), "application/pdf", fileInfo.Name);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
