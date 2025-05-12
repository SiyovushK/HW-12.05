using Domain.DTOs.AuthDtos;
using Domain.DTOs.AuthDTOs;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IAuthService
{
    Task<Response<string>> Register(RegisterDto registerDto);
    Task<Response<TokenDto>> Login(LoginDto loginDto);   
}