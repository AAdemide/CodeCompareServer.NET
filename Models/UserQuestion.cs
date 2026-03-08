using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeCompareServer.Models;

[Table("user_questions")]
public class UserQuestion
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Column("question_name")]
    [JsonPropertyName("question_name")]
    public string? QuestionName { get; set; }

    [Column("question_slug")]
    [JsonPropertyName("question_slug")]
    public string? QuestionSlug { get; set; }

    [Column("question_difficulty")]
    [JsonPropertyName("question_difficulty")]
    public string? QuestionDifficulty { get; set; }

    [Column("unstructured_question_body", TypeName = "text")]
    [JsonPropertyName("unstructured_question_body")]
    public string? UnstructuredQuestionBody { get; set; }

    [Required]
    [Column("structured_question")]
    [JsonPropertyName("structured_question")]
    public bool StructuredQuestion { get; set; }

    [Column("created_at")]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("question_id")]
    [JsonPropertyName("question_id")]
    public int? QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    [JsonIgnore]
    public AllQuestion? Question { get; set; }

    [Required]
    [Column("user_id")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    [JsonIgnore]
    public User User { get; set; } = null!;
}