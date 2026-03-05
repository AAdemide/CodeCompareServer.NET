using System.Text;
using CodeCompareServer.Controllers;
using CodeCompareServer.Data;
using CodeCompareServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CodeCompareServer.Tests;

public class AuthControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var myConfiguration = new Dictionary<string, string>
        {
            {"ACCESS_TOKEN_SECRET", "super_secret_test_key_must_be_long_enough!"}
        }!;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsOkWithToken()
    {
        var context = GetDbContext();
        var controller = new AuthController(context, GetConfiguration());

        var req = new AuthController.RegisterRequest("testUser", "test@test.com", "password123");
        var result = await controller.Register(req) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        
        var users = await context.Users.ToListAsync();
        Assert.Single(users);
        Assert.Equal("testUser", users[0].Username);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var context = GetDbContext();
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123", 10);
        context.Users.Add(new User { Username = "testUser", Email = "test@test.com", Password = hashedPassword });
        await context.SaveChangesAsync();

        var controller = new AuthController(context, GetConfiguration());

        var req = new AuthController.LoginRequest("testUser", "password123");
        var result = await controller.Login(req) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        var context = GetDbContext();
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123", 10);
        context.Users.Add(new User { Username = "testUser", Email = "test@test.com", Password = hashedPassword });
        await context.SaveChangesAsync();

        var controller = new AuthController(context, GetConfiguration());

        var req = new AuthController.LoginRequest("testUser", "wrongpassword");
        var result = await controller.Login(req) as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }
}
