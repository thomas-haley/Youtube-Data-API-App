using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("QueueTask")]
public class QueueTask 
{
    public int Id { get; set; }
    public required AppUser User { get; set; } 
    public required string TaskType {get; set;}
    public List<string>? Videos {get; set;}
    public List<string>? Channels {get; set;}
    public List<string>? Categories {get; set;}
    public int EstimatedTokens {get; set;}
    public required bool Queued {get; set;} = true;
    public required bool Canceled {get; set;} = false;
    public required bool Completed {get; set;} = true;
}