using Microsoft.AspNetCore.Mvc;

namespace OnlineDocumentStore.MVC.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
