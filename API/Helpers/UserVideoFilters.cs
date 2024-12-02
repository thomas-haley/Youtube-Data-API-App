namespace API.Helpers;


public class UserVideoFilters
{
    public int? Channel {get; set;}
    public string? DateWatchedStart {get; set;}
    public string? DateWatchedEnd {get; set;}
    public string? PublishedStart {get; set;}
    public string? PublishedEnd {get; set;}
    public string? Duration {get; set;}
    public string? Views {get; set;}// Always sends operator and a number with _ delim, e.x. "<_5000", ">=_10000", etc.
    public int? Category {get; set;}

}