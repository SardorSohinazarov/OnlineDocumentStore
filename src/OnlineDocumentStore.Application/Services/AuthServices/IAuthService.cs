﻿using OnlineDocumentStore.Application.DataTransferObjects.Auth;

namespace OnlineDocumentStore.Application.Services.AuthServices
{
    public interface IAuthService
    {
        ValueTask<TokenDTO> Register(RegisterDTO registerDTO);
        ValueTask<TokenDTO> Login(LoginDTO loginDTO);
    }
}
