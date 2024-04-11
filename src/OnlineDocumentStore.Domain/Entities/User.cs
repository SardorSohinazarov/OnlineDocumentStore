using Microsoft.AspNetCore.Identity;

namespace OnlineDocumentStore.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string RefreshToken { get; set; }
        public string? Salt { get; set; }
        public DateTime ExpireDate { get; set; }

        public virtual List<Document> Documents { get; set; }
    }
}
