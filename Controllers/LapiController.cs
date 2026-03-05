using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeCompareServer.Data;
using System.Text.Json;

namespace CodeCompareServer.Controllers;

[ApiController]
[Route("lapi")]
public class LapiController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public LapiController(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("LapiClient");
    }

    private int GetUserId()
    {
        var idClaim = User.FindFirst("id")?.Value;
        return int.TryParse(idClaim, out int id) ? id : throw new UnauthorizedAccessException();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Valid routes are: /daily, /random, /:slug");
    }

    [HttpGet("daily")]
    public async Task<IActionResult> Daily()
    {
        try
        {
            var response = await _httpClient.GetAsync("/dailyQuestion");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("data", out var data) && 
                data.TryGetProperty("activeDailyCodingChallengeQuestion", out var dailyQuestion))
            {
                return Ok(dailyQuestion);
            }
            return Ok(document.RootElement);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("There was an error retrieving the daily question: " + ex);
            return StatusCode(500, new { message = $"There was an error retrieving the daily question:  {ex.Message}" });
        }
    }

    [HttpGet("random")]
    public async Task<IActionResult> RandomQuestion()
    {
        try
        {
            var randomQuestion = await _context.AllQuestions
                .OrderBy(q => EF.Functions.Random())
                .FirstOrDefaultAsync();

            if (randomQuestion == null) return NotFound(new { message = "No questions in DB" });

            var response = await _httpClient.GetAsync($"/select?titleSlug={randomQuestion.TitleSlug}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            
            var submissions = await _context.Submissions
                .Include(s => s.UserQuestion)
                .Where(s => s.UserQuestion.QuestionId == randomQuestion.Id)
                .Select(s => new {
                    s.Id, s.SubmissionAnalyses, s.CreatedAt, s.UserQuestionId,
                    question_id = s.UserQuestion.QuestionId,
                    user_question_id = s.UserQuestion.Id
                })
                .ToListAsync();

            return Ok(new { question = document.RootElement, submissions });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("There was an error retrieving a random question: " + ex);
            return StatusCode(500, new { message = $"There was an error retrieving a random question:  {ex.Message}" });
        }
    }

    [Authorize]
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/select?titleSlug={slug}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { message = $"Could not find question with titleSlug:  {slug}" });
            }
            
            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);

            var userId = GetUserId();
            var userQuestion = await _context.UserQuestions
                .FirstOrDefaultAsync(uq => uq.QuestionSlug == slug && uq.UserId == userId);

            if (userQuestion != null)
            {
                var submissions = await _context.Submissions
                    .Where(s => s.UserQuestionId == userQuestion.Id)
                    .ToListAsync();

                return Ok(new { userQuestionId = userQuestion.Id, question = document.RootElement, submissions });
            }
            
            return Ok(new { userQuestionId = (int?)null, question = document.RootElement, submissions = new object[0] });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("There was an error retrieving the question: " + ex);
            return StatusCode(500, new { message = $"There was an error retrieving the question:  {ex.Message}" });
        }
    }
}
