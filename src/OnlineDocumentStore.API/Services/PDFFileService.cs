using Microsoft.AspNetCore.Http.Extensions;
using OnlineDocumentStore.API.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace OnlineDocumentStore.API.Services
{
    public class PDFFileService : IPDFFileService
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IQRCodeService _qrCodeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PDFFileService(IFileService fileService, IWebHostEnvironment webHostEnvironment, IQRCodeService qrCodeService, IHttpContextAccessor httpContextAccessor)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _qrCodeService = qrCodeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<string> AddPhotoAsync(PDFFile pdfFile)
        {
            // PDF faylni saqlash
            var newPDFFileName = Guid.NewGuid().ToString() + ".pdf";
            var newPDFFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "EditedFiles", newPDFFileName);

            var path = await _fileService.UploadAsync(pdfFile.File, "UploadedFiles");

            var qrCodeImage = await _qrCodeService.GenerateQRCodeAsync(GetDownloaderLink(newPDFFileName));

            PdfDocument pdf = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
            pdf.Info.Title = pdfFile.File.Name;

            // Rasmni MemoryStream orqali XImagega o'tkazish
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                PdfPage page = pdf.AddPage();

                // Sahifada chizish uchun XGraphics obyekti
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image = XImage.FromStream(stream);
                gfx.DrawImage(image, 0, 0, page.Width, page.Width);
            }

            pdf.Save(newPDFFilePath);

            return newPDFFilePath;
        }

        private string GetDownloaderLink(string path)
        {
            string url = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();

            url = _httpContextAccessor.HttpContext.Request.Scheme + "://" +
                _httpContextAccessor.HttpContext.Request.Host +
                "/api/PDFEditor/DownloadFile/EditedFiles%5C" +
                path;

            return url;
        }
    }
}
