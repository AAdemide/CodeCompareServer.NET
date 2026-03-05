using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCompareServer.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = null!;

    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password")]
    public string Password { get; set; } = null!;
}
