using OnlineDocumentStore.Application.Abstractions.Repositories;
using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Infrastructure.Repositories
{
    public class DocumentRepository : BaseRepsitory<Document>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext context)
            : base(context) { }
    }
}
