using API.DTOs;
using API.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.TagHelpers;


namespace API.Services;

public class HTMLFileParser : IHTMLFileParser
{

    public List<HTMLDataDTO> ParseHTML(IFormFile file){
        var doc = new HtmlDocument();
        doc.Load(file.OpenReadStream());
        List<HTMLDataDTO> dataList = [];
        var anchorNodes = doc.DocumentNode.SelectNodes("//div/a").ToList();
        foreach(var anchor in anchorNodes){
            dataList.Add(new HTMLDataDTO{
                Href = anchor.GetAttributeValue("href", "")
            });
        }
        return dataList;
    }
}
