namespace OnlineDocumentStore.API.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
            => _webHostEnvironment = webHostEnvironment;

        public async ValueTask<string> Upload(IFormFile file)
        {
            string filePath = "";
            string fileName = "";
            try
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                filePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadedFiles", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on Uploading proccess");
            }
        }
    }
}
