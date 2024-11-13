using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Channel")]
public class AppChannel 
{
    public int Id { get; set; }
    public required string API_Id { get; set; } //channelId from API
    public required string Title {get; set;}//channelTitle from video|list or from channel|list
    public string? Thumbnail {get; set;}//Can only get from channel|list API... eventual feature
}