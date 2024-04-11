using OnlineDocumentStore.Application.DataTransferObjects.Auth;
using OnlineDocumentStore.Domain.Entities;

namespace OnlineDocumentStore.Application.Services.JWTServices
{
    public interface IJWTService
    {
        TokenDTO GenerateToken(User user);
        ValueTask<TokenDTO> RefreshTokenAsync(TokenDTO tokenDTO);
    }
}
