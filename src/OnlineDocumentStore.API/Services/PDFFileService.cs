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

        public PDFFileService(IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async ValueTask<string> AddPhoto(PDFFile pdfFile)
        {
            var path = await _fileService.Upload(pdfFile.File);
            // Yangi PDF hujjatini yaratish
            PdfDocument document = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
            document.Info.Title = "Rasm bilan PDF";

            // Yangi sahifani hujjatga qo'shish
            PdfPage page = document.AddPage();

            // Sahifada chizish uchun XGraphics obyekti
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Rasmni yuklash
            XImage image = XImage.FromFile("D:\\Projects\\OnlineDocumentStore\\src\\OnlineDocumentStore.API\\wwwroot\\Images\\tog.jpg"); // Rasmning yo'lini ko'rsating

            // Rasmi sahifaga joylashtirish
            gfx.DrawImage(image, 0, 0, 200, 150); // Rasmini o'lchamini 3 ga kamaytiramiz

            // PDF faylni saqlash
            var newPDFFileName = Guid.NewGuid().ToString() + ".pdf";
            var newPDFFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "EditedFiles", newPDFFileName);
            document.Save(newPDFFilePath);

            document.Dispose();

            return "/EditedFiles/" + newPDFFileName;
        }
    }
}
