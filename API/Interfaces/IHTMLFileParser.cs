using API.DTOs;

namespace API.Interfaces;

public interface IHTMLFileParser
{
    public List<HTMLDataDTO> ParseHTML(IFormFile file);
}