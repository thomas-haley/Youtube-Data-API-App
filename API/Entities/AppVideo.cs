using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Video")]
public class AppVideo 
{
    public int Id { get; set; }
    public required string API_Id { get; set; } //"v" query param from video url
    public required string Title {get; set;} 
    public DateTime? Published {get; set;} //From API
    public AppChannel? Channel {get; set;} //From API
    public int? ChannelId {get; set;} //From API
    public AppCategory? Category{get; set;}//From API
    public int? CategoryId{get; set;}//From API

    public List<string>? Topics{get; set;}//From API

    public bool Queued {get; set;} = true;

    public bool Retrieved {get; set;} = false;

}