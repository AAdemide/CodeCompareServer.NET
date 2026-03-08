using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeCompareServer.Models;

[Table("submissions")]
public class Submission
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [Column("submission_analyses", TypeName = "text")]
    [JsonPropertyName("submission_analyses")]
    public string SubmissionAnalyses { get; set; } = null!;

    [Column("created_at")]
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Required]
    [Column("user_question_id")]
    [JsonPropertyName("user_question_id")]
    public int UserQuestionId { get; set; }

    [ForeignKey(nameof(UserQuestionId))]
    [JsonIgnore]
    public UserQuestion UserQuestion { get; set; } = null!;
}