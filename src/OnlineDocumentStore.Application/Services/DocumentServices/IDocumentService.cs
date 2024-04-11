using OnlineDocumentStore.Application.ViewModels;

namespace OnlineDocumentStore.Application.Services.DocumentServices
{
    public interface IDocumentService
    {
        ValueTask<DocumentViewModel> GetByIdAsync(Guid id);
    }
}
