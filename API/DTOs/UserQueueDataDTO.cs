
using API.Interfaces;

namespace API.DTOs;

public class UserQueueDataDTO
{
    public required int Id {get; set;}
    public required UserDataDTO User {get; set;}
    public required string TaskType {get; set;}
    public List<string>? Videos {get; set;}
    public List<string>? Channels {get; set;}
    public List<string>? Categories {get; set;}

}
