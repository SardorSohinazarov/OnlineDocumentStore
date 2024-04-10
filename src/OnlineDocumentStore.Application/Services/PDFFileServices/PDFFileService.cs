using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using OnlineDocumentStore.Application.Abstractions.Repositories;
using OnlineDocumentStore.Application.Models;
using OnlineDocumentStore.Application.Services.FileServices;
using OnlineDocumentStore.Application.Services.QRCodeServices;
using OnlineDocumentStore.Domain.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Security.Claims;

namespace OnlineDocumentStore.Application.Services.PDFFileServices
{
    public class PDFFileService : IPDFFileService
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IQRCodeService _qrCodeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocumentRepository _documentRepository;

        public PDFFileService(
            IFileService fileService,
            IWebHostEnvironment webHostEnvironment,
            IQRCodeService qrCodeService,
            IHttpContextAccessor httpContextAccessor,
            IDocumentRepository documentRepository)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _qrCodeService = qrCodeService;
            _httpContextAccessor = httpContextAccessor;
            _documentRepository = documentRepository;
        }

        public async ValueTask<string> AddPhotoAsync(PDFFile pdfFile)
        {
            // PDF faylni saqlash
            var newPDFFileName = Guid.NewGuid().ToString() + ".pdf";
            var newPDFFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "EditedFiles", newPDFFileName);

            var path = await _fileService.UploadAsync(pdfFile.File, "UploadedFiles");


            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var document = await _documentRepository.InsertAsync(new Document()
            {
                Name = pdfFile.File.FileName,
                EditedFilePath = newPDFFilePath,
                UploadedFilePath = path,
                UploadedDate = DateTime.Now,
                UserId = Guid.Parse(userId)
            });

            var qrCodeImage = await _qrCodeService.GenerateQRCodeAsync(GetDownloaderLink(document.Id.ToString()));

            PdfDocument pdf = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
            pdf.Info.Title = pdfFile.File.Name;

            // Rasmni MemoryStream orqali XImagega o'tkazish
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                PdfPage page = pdf.Pages[0];

                // Sahifada chizish uchun XGraphics obyekti
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image = XImage.FromStream(stream);

                var y = 1222.0 * page.Height / 1754.0;
                var x = 101.0 * page.Width / 1241;
                var length = 170.0 * page.Height / 1754.0;
                gfx.DrawImage(image, x, y, length, length);
            }

            pdf.Save(newPDFFilePath);

            return newPDFFilePath;
        }

        public async ValueTask<string> AddPhotoAsync(PDFFile pdfFile, double? x, double? y, double? length)
        {

            if (x is null && y is null && length is null)
                return await AddPhotoAsync(pdfFile);

            double x1 = (double)x;
            double y1 = (double)y;
            double length1 = (double)length;

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

                PdfPage page = pdf.Pages[0];

                // Sahifada chizish uchun XGraphics obyekti
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XImage image = XImage.FromStream(stream);

                gfx.DrawImage(image, x1, y1, length1, length1);
            }

            pdf.Save(newPDFFilePath);

            return newPDFFilePath;
        }

        private string GetDownloaderLink(string id)
        {
            string url = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();

            url = _httpContextAccessor.HttpContext.Request.Scheme + "://" +
                _httpContextAccessor.HttpContext.Request.Host +
                "/api/Documents?id=" +
                id;

            return url;
        }
    }
}
