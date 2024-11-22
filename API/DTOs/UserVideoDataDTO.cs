using System;

namespace API.DTOs;

public class UserVideoDataDTO
{
    public required DateTime Watched { get; set; }
    public required int VideoId {get; set;}
    public required VideoDataDTO Video {get; set;}
}

