namespace OnlineDocumentStore.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UploadedDate { get; set; }
        public int UserId { get; set; }
        public string UploadedFilePath { get; set; }
        public string EditedFilePath { get; set; }

        public virtual User User { get; set; }
    }
}
