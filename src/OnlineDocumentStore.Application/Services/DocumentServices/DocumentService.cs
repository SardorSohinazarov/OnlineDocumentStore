using OnlineDocumentStore.Application.Abstractions.Repositories;
using OnlineDocumentStore.Application.ViewModels;
using OnlineDocumentStore.Domain.Exceptions;

namespace OnlineDocumentStore.Application.Services.DocumentServices
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
            => _documentRepository = documentRepository;

        public async ValueTask<DocumentViewModel> GetByIdAsync(Guid id)
        {
            var document = await _documentRepository.SelectByIdAsync(id);

            if (document is null)
                throw new NotFoundException($"Document not found with this id:{id}");

            return new DocumentViewModel()
            {
                Id = id,
                Name = document.Name,
                UploadedDate = document.UploadedDate,
                UploadedFilePath = document.UploadedFilePath,
                EditedFilePath = document.EditedFilePath,
                User = new UserViewModel()
                {
                    Id = document.User.Id,
                    FullName = document.User.FullName,
                    Email = document.User.Email,
                    PhoneNumber = document.User.PhoneNumber,
                }
            };
        }
    }
}
