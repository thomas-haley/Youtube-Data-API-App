using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VideoAddedDTO
{
    public required bool NewVideo {get; set;}
    public required bool NewChannel {get; set;}
    
}
