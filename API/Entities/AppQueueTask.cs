using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("QueueTask")]
public class QueueTask 
{
    public int Id { get; set; }
    public required AppUser User { get; set; } 
    public required string TaskType {get; set;}
    public List<AppVideo>? Videos {get; set;}
    public List<AppChannel>? Channels {get; set;}
    public List<AppCategory>? Categories {get; set;}
    public required int EstimatedTokens {get; set;}
    public required bool Queued {get; set;} = true;
    public required bool Caneled {get; set;} = false;
    public required bool Completed {get; set;} = true;
}