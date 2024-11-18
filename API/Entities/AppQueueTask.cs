using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class AppQueueTask 
{
    public int Id { get; set; }
    public required AppUser User { get; set; } 
    public required string TaskType {get; set;}
    public List<string>? Videos {get; set;}
    public List<string>? Channels {get; set;}
    public List<string>? Categories {get; set;}
    public int EstimatedTokens {get; set;}
    public bool Queued {get; set;} = false;
    public bool Canceled {get; set;} = false;
    public bool Completed {get; set;} = true;
}