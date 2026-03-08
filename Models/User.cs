using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeCompareServer.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [Required]
    [Column("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password")]
    [JsonIgnore]
    public string Password { get; set; } = null!;
}