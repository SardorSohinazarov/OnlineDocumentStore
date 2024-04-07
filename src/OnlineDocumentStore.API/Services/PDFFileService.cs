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
        public PDFFileService(IFileService fileService, IWebHostEnvironment webHostEnvironment, IQRCodeService qrCodeService)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _qrCodeService = qrCodeService;
        }

        public async ValueTask<string> AddPhoto(PDFFile pdfFile)
        {
            // PDF faylni saqlash
            var newPDFFileName = Guid.NewGuid().ToString() + ".pdf";
            var newPDFFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "EditedFiles", newPDFFileName);

            var path = await _fileService.Upload(pdfFile.File, "UploadedFiles");

            var qrCodeImage = await _qrCodeService.GenerateQRCode(path);

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
    }
}
