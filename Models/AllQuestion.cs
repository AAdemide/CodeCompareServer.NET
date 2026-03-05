using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("all_questions")]
public class AllQuestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("acRate")]
    public float AcRate { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Required]
    [Column("titleSlug")]
    public string TitleSlug { get; set; } = null!;

    [Required]
    [Column("difficulty")]
    public string Difficulty { get; set; } = null!;

    [Required]
    [Column("topicTags", TypeName = "json")]
    public string TopicTags { get; set; } = null!;

    [Required]
    [Column("hasSolution")]
    public bool HasSolution { get; set; }
}
