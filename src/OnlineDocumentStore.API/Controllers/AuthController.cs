using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineDocumentStore.Application.DataTransferObjects.Auth;
using OnlineDocumentStore.Application.Services.AuthServices;
using OnlineDocumentStore.Application.Services.JWTServices;

namespace OnlineDocumentStore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJWTService _jwtService;

        public AuthController(
            IAuthService authService,
            IJWTService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
            => Ok(await _authService.Login(loginDTO));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
            => Ok(await _authService.Register(registerDTO));

        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
            => Ok(await _jwtService.RefreshTokenAsync(tokenDTO));

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckUser()
            => Ok();
    }
}
