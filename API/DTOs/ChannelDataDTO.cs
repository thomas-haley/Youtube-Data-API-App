namespace API.DTOs;

public class ChannelDataDTO
{
    public required int Id {get; set;}
    public required string API_Id {get; set;}
    public required string Title {get; set;}
    public string? Thumbnail {get; set;}
    
}
