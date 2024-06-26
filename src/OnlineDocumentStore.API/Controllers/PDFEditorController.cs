﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.DataTransferObjects;
using OnlineDocumentStore.Application.Services.PDFFileServices;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PDFEditorController : ControllerBase
    {
        private readonly IPDFFileService _pdfFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PDFEditorController(
            IPDFFileService pdfFileService,
            IWebHostEnvironment webHostEnvironment)
        {
            _pdfFileService = pdfFileService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddQRCodeAsync([FromForm] PDFFile pdfFile, double? x, double? y, double? length)
        {
            var editedFile = await _pdfFileService.AddPhotoAsync(pdfFile, x, y, length);

            if (!System.IO.File.Exists(editedFile))
                throw new Exception("File not found");

            var fileInfo = new System.IO.FileInfo(editedFile);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(editedFile), "application/pdf", fileInfo.Name);
        }

        [HttpGet("{path}")]
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
