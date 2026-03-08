using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeCompareServer.Models;

[Table("collections")]
public class Collection
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [Column("collection_name")]
    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = null!;
}