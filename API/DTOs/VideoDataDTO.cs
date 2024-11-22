namespace API.DTOs;

public class VideoDataDTO
{
    public required string API_Id {get; set;}
    public required string Title {get; set;}
    public required ChannelDataDTO Channel {get; set;}
    public DateTime? Published {get; set;}
    public string? Category {get; set;}
    public List<string>? Topics {get; set;}
    public string? Thumbnail {get; set;}
    public int? Views {get; set;}
    public DateTime? DataFetched {get; set;}
    public required bool Retrieved {get; set;}
    
}
