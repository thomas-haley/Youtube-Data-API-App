using System.Globalization;
using System.Text.RegularExpressions;
using API.DTOs;
using API.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.TagHelpers;


namespace API.Services;

public class HTMLFileParser : IHTMLFileParser
{

    private Regex videoHrefRegex = new Regex(@"(?:.(?!=))+$");
    private Regex channelHrefRegex = new Regex(@"(?:.(?!\/))+$");
    private Regex watchDateRegex = new Regex(@"(?:.(?!<br>))+$");

    private Dictionary<string, string>months = new Dictionary<string, string>()
    {
        {"jan", "01"},
        {"feb", "02"},
        {"mar", "03"},
        {"apr", "04"},
        {"may", "05"},
        {"jun", "06"},
        {"jul", "07"},
        {"aug", "08"},
        {"sep", "09"},
        {"oct", "10"},
        {"nov", "11"},
        {"dec", "12"}
    };

    public List<VideoDTO> ParseHTML(IFormFile file){
        var doc = new HtmlDocument();
        doc.Load(file.OpenReadStream());
        List<VideoDTO> dataList = [];
        var videoNodes = doc.DocumentNode.SelectNodes("/html/body/div/div/div").ToList();
        foreach(var video in videoNodes){
            // Console.WriteLine("Passing in:");
            // Console.WriteLine(video.InnerLength);
            var videoData = this.extractVideoData(video);
            if(videoData != null) dataList.Add(videoData);
        }
        return dataList;
    }

    private VideoDTO? extractVideoData(HtmlNode videoNode){

        try{    
            // Console.WriteLine(videoNode.InnerHtml);
            var docMoc = new HtmlDocument();
            docMoc.LoadHtml(videoNode.OuterHtml);
            var anchors = docMoc.DocumentNode.SelectNodes("//a");
            // Console.WriteLine(anchors[0].InnerHtml);
            // string? videoHref;
            var videoHref = anchors[0].GetAttributeValue("href", "");
            if(videoHref == null || videoHref == ""){
                Console.WriteLine("Unable to extract video HREF");
                return null;
            }
            

            string videoId = this.videoHrefRegex.Matches(videoHref).ToList().First().Value.Substring(1);
            string videoTitle = anchors[0].InnerHtml;
            string? channelHref;
            channelHref = anchors[1].GetAttributeValue("href", "");
            if(channelHref == null || channelHref == ""){
                Console.WriteLine("Unable to find channel HREF");
                return null;
            }
            string channelId = this.channelHrefRegex.Matches(channelHref).ToList().Last().Value.Substring(1);
            string channelName = anchors[1].InnerHtml;

            string dateString = this.watchDateRegex.Matches(videoNode.ChildNodes[1].InnerHtml).ToList().Last().Value.Substring(4);
            var watchDate = this.buildWatchDateFromString(dateString);
            
            if(watchDate == null) return null;
            
            return new VideoDTO{
                API_Id = videoId,
                Title = videoTitle,
                ChannelId = channelId,
                ChannelName = channelName,
                Watched =  (DateTime)watchDate
            };
        } catch (Exception ex){
            Console.WriteLine("GOT ERROR EXTRACTING DATA FROM FOLLOWING HTML ELEMENT:\n\n");
            Console.WriteLine(videoNode.InnerHtml);
            return null;
        }
        
    }

    private string? findMonth(string monthString){
        foreach(var month in this.months){
            if(monthString.ToLower().Contains(month.Key)){
                return month.Value;
            }
        }

        return null;
    }

    private DateTime? buildWatchDateFromString(string dateWatchedString)
    {
        var strSplit = dateWatchedString.Split(" ");
        var monthString = this.findMonth(strSplit[0]);
        var dateString = strSplit[1].Remove(strSplit[1].Length - 1);
        var yearString = strSplit[2].Remove(strSplit[2].Length - 1);
        if(monthString == null) return null;
        var formattedString = yearString + "-" + monthString + "-" + dateString + " " + strSplit[3];

        var timeObj = DateTime.Parse(formattedString);
        return timeObj;
    }

    private void debugHTMLDataDTO(VideoDTO videoDTO){
        Console.WriteLine($"Video ID: {videoDTO.API_Id}");
        Console.WriteLine($"Video Title: {videoDTO.Title}");
        Console.WriteLine($"Channel ID: {videoDTO.ChannelId}");
        Console.WriteLine($"Channel Name: {videoDTO.ChannelName}");
        Console.WriteLine($"Watched: {videoDTO.Watched}");
    }
}
