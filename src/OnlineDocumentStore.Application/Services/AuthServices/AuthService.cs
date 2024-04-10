using Microsoft.EntityFrameworkCore;
using OnlineDocumentStore.Application.Abstractions;
using OnlineDocumentStore.Application.Models.Auth;
using OnlineDocumentStore.Application.Services.Halpers;
using OnlineDocumentStore.Application.Services.JWTServices;
using OnlineDocumentStore.Domain.Entities;
using OnlineDocumentStore.Domain.Exceptions;

namespace OnlineDocumentStore.Application.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTService _jwtService;

        public AuthService(IAppDbContext context, IPasswordHasher passwordHasher, IJWTService jwtService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async ValueTask<TokenDTO> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Email == loginDTO.Email);

            if (user == null)
                throw new NotFoundException($"User not found with {loginDTO.Email} email");

            if (!_passwordHasher.Verify(user.PasswordHash, loginDTO.Password, user.Salt))
                throw new ValidationException("Username or password is not valid");

            return _jwtService.GenerateToken(user);
        }

        public async ValueTask<TokenDTO> Register(RegisterDTO registerDTO)
        {
            var salt = Guid.NewGuid().ToString();
            var passwordHash = _passwordHasher.Encrypt(registerDTO.Password, salt);

            var user = new User
            {
                FullName = registerDTO.FullName,
                PasswordHash = passwordHash,
                PhoneNumber = registerDTO.PhoneNumber,
                Email = registerDTO.Email,
                Salt = salt,
                RefreshToken = Guid.NewGuid().ToString(),
                ExpireDate = DateTime.Now.AddDays(7),
            };

            var entityEntry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(entityEntry.Entity);
        }
    }
}
