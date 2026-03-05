using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("submissions")]
public class Submission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("submission_analyses", TypeName = "text")]
    public string SubmissionAnalyses { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Required]
    [Column("user_question_id")]
    public int UserQuestionId { get; set; }

    [ForeignKey(nameof(UserQuestionId))]
    public UserQuestion UserQuestion { get; set; } = null!;
}
