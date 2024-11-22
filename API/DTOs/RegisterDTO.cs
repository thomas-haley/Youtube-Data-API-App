using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    [StringLength(24, MinimumLength = 6)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(24, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

}
