using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using todoapi.API.model;
using todoapi.API.users;
namespace todoapi.API.service;

public class IdentityService(JwtConfiguration config)
{
    private readonly JwtConfiguration _config = config;

    public async Task<string> GenerateToken(UserInfo user)
    {
        await Task.Delay(100); // Simulate a database call

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName), 
            new Claim(JwtRegisteredClaimNames.Sub,user.userId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_config.ExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}