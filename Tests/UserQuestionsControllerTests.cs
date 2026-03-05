using System.Security.Claims;
using CodeCompareServer.Controllers;
using CodeCompareServer.Data;
using CodeCompareServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodeCompareServer.Tests;

public class UserQuestionsControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        
        context.AllQuestions.Add(new AllQuestion { 
            Id = 10, TitleSlug = "test-question", Difficulty = "Easy",
            AcRate = 50.0f, TopicTags = "[]", HasSolution = true, Title = "Test Q" 
        });
        context.SaveChanges();
        return context;
    }

    private UserQuestionsController GetController(AppDbContext context, int userId = 1)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim("id", userId.ToString())
        }, "mock"));

        var controller = new UserQuestionsController(context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            }
        };

        return controller;
    }

    [Fact]
    public async Task Index_ReturnsQuestionsForUser()
    {
        var context = GetDbContext();
        context.UserQuestions.Add(new UserQuestion { UserId = 1, QuestionName = "My Q" });
        context.UserQuestions.Add(new UserQuestion { UserId = 2, QuestionName = "Other Q" });
        await context.SaveChangesAsync();

        var controller = GetController(context, 1);
        var result = await controller.Index() as OkObjectResult;

        Assert.NotNull(result);
        var questions = Assert.IsAssignableFrom<List<UserQuestion>>(result.Value);
        Assert.Single(questions);
        Assert.Equal("My Q", questions[0].QuestionName);
    }

    [Fact]
    public async Task AddOne_StructuredQuestion_SavesToDb()
    {
        var context = GetDbContext();
        var controller = GetController(context, 1);

        var req = new UserQuestionsController.AddQuestionRequest(true, "Test Q", "test-question", "Easy", null);
        var result = await controller.AddOne(req) as ObjectResult;

        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);

        var userQuestions = await context.UserQuestions.ToListAsync();
        Assert.Single(userQuestions);
        Assert.Equal("test-question", userQuestions[0].QuestionSlug);
    }
}
