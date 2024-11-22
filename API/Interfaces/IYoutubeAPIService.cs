using Google.Apis.YouTube.v3.Data;

namespace API.Interfaces;


public interface IYoutubeAPIService{
    public ChannelListResponse ListChannels(List<string> channels);
    public VideoListResponse ListVideos(List<string> videos);
}