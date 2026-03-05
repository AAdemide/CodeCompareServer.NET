using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("collection_questions")]
public class CollectionQuestion
{
    [Key]
    [Column("question_id")]
    public int QuestionId { get; set; }

    [Required]
    [Column("question_name")]
    public string QuestionName { get; set; } = null!;

    [Required]
    [Column("question_slug")]
    public string QuestionSlug { get; set; } = null!;

    [Required]
    [Column("question_difficulty")]
    public string QuestionDifficulty { get; set; } = null!;

    [Column("question_tags", TypeName = "json")]
    public string? QuestionTags { get; set; }

    [Required]
    [Column("collections_id")]
    public int CollectionsId { get; set; }

    [ForeignKey(nameof(CollectionsId))]
    public Collection Collection { get; set; } = null!;
}
