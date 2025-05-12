using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.AuthDTOs;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using Domain.DTOs.AuthDtos;

namespace Infrastructure.Services;

public class AuthService(
    DataContext context,
    IConfiguration config) : IAuthService
{
    private readonly PasswordHasher<Student> _hasher = new();

    public async Task<Response<string>> Register(RegisterDto registerDto)
    {
        var existingUser = await context.Students.FirstOrDefaultAsync(s => s.UserName == registerDto.UserName);
        if (existingUser != null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Username already exists");
        }

        var student = new Student
        {
            UserName = registerDto.UserName,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            BirthDate = registerDto.BirthDate
        };

        student.PasswordHash = _hasher.HashPassword(student, registerDto.Password);

        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();

        return new Response<string>("User registered successfully");
    }

    public async Task<Response<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await context.Students.FirstOrDefaultAsync(s => s.UserName == loginDto.UserName);
        if (user == null)
        {
            return new Response<TokenDto>(HttpStatusCode.BadRequest, "Username or password is incorrect");
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return new Response<TokenDto>(HttpStatusCode.BadRequest, "Username or password is incorrect");
        }

        var token = GenerateJwt(user);
        return new Response<TokenDto>(new TokenDto { Token = token });
    }

    private string GenerateJwt(Student student)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, student.StudentId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, student.UserName),
            new Claim("FullName", $"{student.FirstName} {student.LastName}")
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}