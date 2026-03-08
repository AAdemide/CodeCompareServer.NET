using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeCompareServer.Models;

[Table("all_questions")]
public class AllQuestion
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [Column("acRate")]
    [JsonPropertyName("ac_rate")]
    public float AcRate { get; set; }

    [Required]
    [Column("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [Required]
    [Column("titleSlug")]
    [JsonPropertyName("title_slug")]
    public string TitleSlug { get; set; } = null!;

    [Required]
    [Column("difficulty")]
    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; } = null!;

    [Required]
    [Column("topicTags", TypeName = "json")]
    [JsonPropertyName("topic_tags")]
    public string TopicTags { get; set; } = null!;

    [Required]
    [Column("hasSolution")]
    [JsonPropertyName("has_solution")]
    public bool HasSolution { get; set; }
}