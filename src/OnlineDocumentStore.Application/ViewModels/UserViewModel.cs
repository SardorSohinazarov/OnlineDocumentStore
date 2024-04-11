using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Application.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<Document> Documents { get; set; }
    }
}
