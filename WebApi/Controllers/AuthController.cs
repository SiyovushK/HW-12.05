using Domain.DTOs.AuthDtos;
using Domain.DTOs.AuthDTOs;
using Domain.Response;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<Response<string>> Register(RegisterDto dto)
    {
        return await authService.Register(dto);
    }

    [HttpPost("login")]
    public async Task<Response<TokenDto>> Login(LoginDto dto)
    {
        return await authService.Login(dto);
    }
}