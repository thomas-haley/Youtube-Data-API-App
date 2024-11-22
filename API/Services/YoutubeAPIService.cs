using API.Interfaces;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace API.Services;

public class YoutubeAPIService : IYoutubeAPIService
{
    protected YouTubeService _youtubeAPI;
    public YoutubeAPIService()
    {
        var appName = Environment.GetEnvironmentVariable("Youtube_Api_Name");
        var apiKey = Environment.GetEnvironmentVariable("Youtube_Api_Key");
        _youtubeAPI = new YouTubeService(new BaseClientService.Initializer{
           ApplicationName =  appName,
           ApiKey = apiKey
        });
    }

    public VideoListResponse ListVideos(List<string> videos){
        var request = _youtubeAPI.Videos.List(new Repeatable<string>(["contentDetails", "snippet", "topicDetails"]));
        request.Id = new Repeatable<string>(videos);

        var response = request.Execute();

        return response;        
    }

    public ChannelListResponse ListChannels(List<string> channels){
        var request = _youtubeAPI.Channels.List(new Repeatable<string>(["snippet"]));
        request.Id = new Repeatable<string>(channels);

        var response = request.Execute();

        return response;     
    }
}