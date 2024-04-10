namespace OnlineDocumentStore.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string RefreshToken { get; set; }
        public string Salt { get; set; }
        public DateTime ExpireDate { get; set; }

        public virtual List<Document> Documents { get; set; }
    }
}
