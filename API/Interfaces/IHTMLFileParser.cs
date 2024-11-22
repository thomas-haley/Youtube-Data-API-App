using API.DTOs;

namespace API.Interfaces;

public interface IHTMLFileParser
{
    public List<VideoDTO> ParseHTML(IFormFile file);
}