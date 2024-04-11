using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.Services.DocumentServices;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
            => _documentService = documentService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            var document = await _documentService.GetByIdAsync(id);
            return Ok(document);
        }
    }
}
