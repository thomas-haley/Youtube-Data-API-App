using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class VideoDTO
{
    public required string API_Id {get; set;}
    public required string Title {get; set;}
    public required string ChannelId {get; set;}
    public required string ChannelName {get; set;}
    public required DateOnly Watched {get; set;} 
    
}
