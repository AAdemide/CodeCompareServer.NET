using Microsoft.AspNetCore.Mvc;
using CodeCompareServer.Data;
using CodeCompareServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeCompareServer.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public record RegisterRequest(string Username, string Email, string Password);
    public record LoginRequest(string Username, string Password);

    private string GetAccessToken(int id)
    {
        var jwtSecret = _configuration["ACCESS_TOKEN_SECRET"] ?? _configuration["JwtSettings:Secret"];
        var key = Encoding.UTF8.GetBytes(jwtSecret!);

        var claims = new[] { new Claim("id", id.ToString()) };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        try
        {
            var emailRe = new Regex(@"^(([^<>()[\]\.,;:\s@""]+(\.[^<>()[\]\.,;:\s@""]+)*)|("".+""))@(([^<>()[\]\.,;:\s@""]+\.)+[^<>()[\]\.,;:\s@""]{2,})$", RegexOptions.IgnoreCase);
            var passwordRegex = new Regex(@"^(?=.*\d).{5,}$");

            if (string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password) ||
                !emailRe.IsMatch(req.Email) || !passwordRegex.IsMatch(req.Password))
            {
                return BadRequest(new { message = "Invalid registration: all fields must be filled, valid email must be provided and a password must be at least 5 characters including 1 number" });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email || u.Username == req.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "user already exists" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password, 10); // 10 rounds as Node.js did
            var newUser = new User { Username = req.Username, Email = req.Email, Password = hashedPassword };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GetAccessToken(newUser.Id);
            return Ok(new { token, id = newUser.Id });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("couldn't register user: " + ex.Message);
            return StatusCode(500, new { message = "couldn't register user: " + ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user == null)
            {
                return NotFound(new { message = "user not found" });
            }

            if (BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                var token = GetAccessToken(user.Id);
                return Ok(new { token, id = user.Id });
            }
            return Unauthorized(new { message = "incorrect password" });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("couldn't login user: " + ex.Message);
            return StatusCode(500, new { message = "couldn't login user: " + ex.Message });
        }
    }
}
