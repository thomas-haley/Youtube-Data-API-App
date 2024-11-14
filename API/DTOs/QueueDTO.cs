using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs;

public class QueueDTO
{

    [Required]
    public required AppUser User { get; set; }

    [Required]
    public required string TaskType {get; set;}
    
    public List<string>? Videos {get; set;}
    public List<string>? Channels {get; set;}
    public List<string>? Categories {get; set;}

}
