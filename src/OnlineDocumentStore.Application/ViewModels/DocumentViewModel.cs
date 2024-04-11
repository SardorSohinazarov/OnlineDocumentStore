namespace OnlineDocumentStore.Application.ViewModels
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UploadedFilePath { get; set; }
        public string EditedFilePath { get; set; }

        public UserViewModel User { get; set; }
    }
}
