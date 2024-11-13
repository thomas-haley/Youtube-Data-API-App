using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    [StringLength(24, MinimumLength = 8)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(24, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

}
