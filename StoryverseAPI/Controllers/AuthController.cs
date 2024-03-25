using Konscious.Security.Cryptography;
using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StoryverseAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using StoryverseAPI.Data.DTOs.Auth;
using StoryverseAPI.Data.Models;
using System.Security.Cryptography;

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

    [HttpPost("Login")]
    public IActionResult Login(AuthLoginDTO authLoginDTO)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == authLoginDTO.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!VerifyPassword(authLoginDTO.Password, user.Password, user.Salt))
        {
            return Unauthorized();
        }

        var token = GenerateJSONWebToken(user);

        return Ok(new { token });
    }

    [HttpPost("Register")]
    public IActionResult Register(AuthRegisterDTO authRegisterDTO)
    {
        var salt = GenerateSalt();

        var hashedPassword = HashPassword(authRegisterDTO.Password, salt);

        var user = new User { Username = authRegisterDTO.Username, Password = hashedPassword, Salt = salt, Email = authRegisterDTO.Email };
        _db.Users.Add(user);
        _db.SaveChanges();

        var token = GenerateJSONWebToken(user);

        return CreatedAtAction(nameof(Login), new { token });
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

    public static string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            hasher.Salt = saltBytes;
            hasher.DegreeOfParallelism = 8;
            hasher.MemorySize = 65536;
            hasher.Iterations = 4;

            return Convert.ToBase64String(hasher.GetBytes(128));
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        var newHashedPassword = HashPassword(password, salt);

        return hashedPasswordBytes.SequenceEqual(Convert.FromBase64String(newHashedPassword));
    }

    public static string GenerateSalt()
    {
        var bytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Convert.ToBase64String(bytes);
    }
}
