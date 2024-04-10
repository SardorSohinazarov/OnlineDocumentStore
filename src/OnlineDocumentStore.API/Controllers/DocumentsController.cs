using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.Abstractions.Repositories;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentsController(IDocumentRepository documentRepository)
            => _documentRepository = documentRepository;

        [HttpGet]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            var document = await _documentRepository.SelectByIdAsync(id);
            return Ok(document);
        }
    }
}
