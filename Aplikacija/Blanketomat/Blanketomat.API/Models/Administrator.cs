using System.ComponentModel.DataAnnotations;

namespace Blanketomat.API.Models;

public class Administrator
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    public required string Ime { get; set; }

    [MaxLength(50)]
    [EmailAddress]
    public required string Email { get; set; }

    [MaxLength(30)]
    public required string Password { get; set; }
}