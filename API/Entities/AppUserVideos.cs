using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("UserVideo")]

public class AppUserVideos
{
    public required int Id {get; set;}
    public required int UserId {get; set;}
    public required AppUser User { get; set; }

    public required int VideoId {get; set;}
    public required AppVideo Video {get; set;}
}