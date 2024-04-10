namespace OnlineDocumentStore.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadedDate { get; set; }
        public Guid UserId { get; set; }
        public string UploadedFilePath { get; set; }
        public string EditedFilePath { get; set; }

        public virtual User User { get; set; }
    }
}
