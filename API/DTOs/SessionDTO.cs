using System;

namespace API.DTOs;

public class SessionDTO
{
    public required int Id {get; set;}
    public required bool AllowUpload { get; set; }
    public required bool DataUploaded { get; set; }
}
