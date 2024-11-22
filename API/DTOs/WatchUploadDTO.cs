using System;

namespace API.DTOs;

public class WatchUploadDTO
{
    public required IFormFile File {get; set;}
}
