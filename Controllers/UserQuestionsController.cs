using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeCompareServer.Data;
using CodeCompareServer.Models;

namespace CodeCompareServer.Controllers;

[ApiController]
[Route("userQuestions")]
[Authorize]
public class UserQuestionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserQuestionsController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var idClaim = User.FindFirst("id")?.Value;
        return int.TryParse(idClaim, out int id) ? id : throw new UnauthorizedAccessException();
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = GetUserId();
            var questions = await _context.UserQuestions
                .Where(uq => uq.UserId == userId)
                .ToListAsync();
            return Ok(questions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving questions:{ex}");
            return BadRequest(new { message = $"Error retrieving questions:{ex.Message}" });
        }
    }

    public record AddQuestionRequest(bool StructuredQuestion, string? QuestionName, string? QuestionSlug, string? QuestionDifficulty, string? UnstructuredQuestionBody);

    [HttpPost]
    public async Task<IActionResult> AddOne([FromBody] AddQuestionRequest req)
    {
        try
        {
            var userId = GetUserId();
            UserQuestion newQuestion;

            if (req.StructuredQuestion)
            {
                if (string.IsNullOrEmpty(req.QuestionName) || string.IsNullOrEmpty(req.QuestionSlug) || string.IsNullOrEmpty(req.QuestionDifficulty))
                {
                    return BadRequest("All fields are required");
                }

                var allQuestion = await _context.AllQuestions.FirstOrDefaultAsync(q => q.TitleSlug == req.QuestionSlug);
                if (allQuestion == null) return BadRequest(new { message = "could not find question" });

                newQuestion = new UserQuestion
                {
                    QuestionId = allQuestion.Id,
                    QuestionName = req.QuestionName,
                    QuestionSlug = req.QuestionSlug,
                    QuestionDifficulty = req.QuestionDifficulty,
                    StructuredQuestion = req.StructuredQuestion,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.UserQuestions.Add(newQuestion);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (string.IsNullOrEmpty(req.UnstructuredQuestionBody) || string.IsNullOrEmpty(req.QuestionName))
                {
                    return BadRequest("All fields are required");
                }
                newQuestion = new UserQuestion
                {
                    QuestionName = req.QuestionName,
                    UnstructuredQuestionBody = req.UnstructuredQuestionBody,
                    StructuredQuestion = false,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.UserQuestions.Add(newQuestion);
                await _context.SaveChangesAsync();
            }

            return StatusCode(201, newQuestion);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error creating question: " + ex);
            return BadRequest(new { message = $"Unable to create new one: {ex.Message}" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        try
        {
            var question = await _context.UserQuestions.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound(new { message = "Question with id not found" });
            }

            var submissions = await _context.Submissions
                .Where(s => s.UserQuestionId == id)
                .ToListAsync();

            return Ok(new { question, submissions });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving question:{ex}");
            return BadRequest(new { message = $"Error retrieving question:{ex.Message}" });
        }
    }

    public record AddSubmissionRequest(string SubmissionAnalyses);

    [HttpPost("{id}/submission")]
    public async Task<IActionResult> AddSubmission(int id, [FromBody] AddSubmissionRequest req)
    {
        try
        {
            var newSubmission = new Submission
            {
                UserQuestionId = id,
                SubmissionAnalyses = req.SubmissionAnalyses,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Submissions.Add(newSubmission);
            await _context.SaveChangesAsync();

            return StatusCode(201, newSubmission);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error adding submission:{ex}");
            return BadRequest(new { message = $"Error adding submission:{ex.Message}" });
        }
    }
}
