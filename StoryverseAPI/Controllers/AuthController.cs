using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StoryverseAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using StoryverseAPI.Data.DTOs.Auth;
using StoryverseAPI.Data.Models;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DB _db;
    private readonly string _key;
    private readonly string _issuer;

    public AuthController(DB context)
    {
        _db = context;
        _key = Environment.GetEnvironmentVariable("Jwt_Key");
        _issuer = Environment.GetEnvironmentVariable("Jwt_Issuer");
    }

    private string GenerateJSONWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim("id", user.Id.ToString()),
        };

        var token = new JwtSecurityToken(_issuer,
          _issuer,
          claims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("Login")]
    public IActionResult Login(AuthLoginDTO authLoginDTO)
    {
        // Authenticate the user. This is just a placeholder, replace with your own logic.
        var user = _db.Users.FirstOrDefault(u => u.Username == authLoginDTO.Username && u.Password == authLoginDTO.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = GenerateJSONWebToken(user);

        return Ok(new { token });
    }

    [HttpPost("Register")]
    public IActionResult Register(AuthRegisterDTO authRegisterDTO)
    {
        // Create the user. This is just a placeholder, replace with your own logic.
        var user = new User { Username = authRegisterDTO.Username, Password = authRegisterDTO.Password , Email = authRegisterDTO.Email };
        _db.Users.Add(user);
        _db.SaveChanges();

        var token = GenerateJSONWebToken(user);

        return CreatedAtAction(nameof(Login), new { token });
    }
}
