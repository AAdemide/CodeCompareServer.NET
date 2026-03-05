using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("user_questions")]
public class UserQuestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("question_name")]
    public string? QuestionName { get; set; }

    [Column("question_slug")]
    public string? QuestionSlug { get; set; }

    [Column("question_difficulty")]
    public string? QuestionDifficulty { get; set; }

    [Column("unstructured_question_body", TypeName = "text")]
    public string? UnstructuredQuestionBody { get; set; }

    [Required]
    [Column("structured_question")]
    public bool StructuredQuestion { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("question_id")]
    public int? QuestionId { get; set; }
    
    [ForeignKey(nameof(QuestionId))]
    public AllQuestion? Question { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
