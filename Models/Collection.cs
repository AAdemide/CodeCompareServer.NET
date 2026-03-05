using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("collections")]
public class Collection
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("collection_name")]
    public string CollectionName { get; set; } = null!;
}
