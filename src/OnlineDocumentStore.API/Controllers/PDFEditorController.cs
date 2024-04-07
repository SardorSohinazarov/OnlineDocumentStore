﻿using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.API.Models;
using OnlineDocumentStore.API.Services;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PDFEditorController : ControllerBase
    {
        private readonly IPDFFileService _pdfFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PDFEditorController(IPDFFileService pdfFileService, IWebHostEnvironment webHostEnvironment)
        {
            _pdfFileService = pdfFileService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> AddQRCodeAsync([FromForm] PDFFile pdfFile)
        {
            var editedFile = await _pdfFileService.AddPhotoAsync(pdfFile);

            if (!System.IO.File.Exists(editedFile))
            {
                // Return a 404 Not Found error if the file does not exist
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(editedFile);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(editedFile), "application/pdf", fileInfo.Name);
            return Ok(editedFile);
        }

        [HttpGet("{path}")]
        public async Task<IActionResult> DownloadFileAsync(string path)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string outputFilePath = Path.Combine(webRootPath, path);

            if (!System.IO.File.Exists(outputFilePath))
            {
                // Return a 404 Not Found error if the file does not exist
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(outputFilePath);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(outputFilePath), "application/pdf", fileInfo.Name);
        }
    }
}
