using Microsoft.AspNetCore.Mvc;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Qalesiz()
            => Ok("Yaxshi rahmat");
    }
}
