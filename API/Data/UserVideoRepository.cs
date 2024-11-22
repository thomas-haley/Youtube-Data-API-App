using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserVideoRepository(DataContext context, IMapper mapper) : IUserVideoRepository
{
    private bool channelShellCreated = false;
    private bool videoShellCreated = false;
    public async Task<VideoAddedDTO> CreateVideoReferencesAsync(AppUser user, VideoDTO videoData){
        this.channelShellCreated = false;
        this.videoShellCreated = false;

        var channel = await this.ChannelExistsByAPIID(videoData.ChannelId) ?? await this.CreateChannelShell(videoData.ChannelId, videoData.ChannelName);
        if(channel == null) throw new Exception("Unable to fetch AppChannel when creating references");

        var video = await this.VideoExistsByAPIID(videoData.API_Id) ?? await this.CreateVideoShell(videoData.API_Id, videoData.Title, channel);
        if(video == null) throw new Exception("Unable to fetch AppVideo when creating references");


        context.UserVideos.Add(new AppUserVideos{
                UserId = user.Id,
                User = user,
                VideoId = video.Id,
                Video = video,
                Watched = videoData.Watched
        });

        return new VideoAddedDTO{
            NewChannel = this.channelShellCreated,
            NewVideo = this.videoShellCreated
        };
    }
    public void Update(AppUserVideos userVideo)
    {
        context.Entry(userVideo).State = EntityState.Modified;
    }

    public bool CreateUserVideo(AppUserVideos userVideo){
        var entity = context.UserVideos.Add(userVideo);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }

    private async Task<AppVideo?> VideoExistsByAPIID(string apiID){
        return await context.Videos.FirstOrDefaultAsync((AppVideo video) => video.API_Id == apiID);
    }

    private async Task<AppVideo?> CreateVideoShell(string apiID, string channelName, AppChannel channel){
        this.videoShellCreated = true;
        context.Videos.Add(new AppVideo{
            API_Id = apiID,
            Title = channelName,
            ChannelId = channel.Id,
            Channel = channel
        });
        await context.SaveChangesAsync(); 
        return await this.VideoExistsByAPIID(apiID);
    }

    private async Task<AppChannel?> ChannelExistsByAPIID(string apiID){
        return await context.Channels.FirstOrDefaultAsync((AppChannel channel) => channel.API_Id == apiID);
    }

    private async Task<AppChannel?> CreateChannelShell(string apiID, string channelName){
        this.channelShellCreated = true;
        context.Channels.Add(new AppChannel{
            API_Id = apiID,
            Title = channelName
        });
        await context.SaveChangesAsync();
        return await this.ChannelExistsByAPIID(apiID);
    }

        public async Task<PagedList<UserVideoDataDTO>> GetUserVideosAsync(UserVideoParams userVideoParams, int userID)
    {   
        var query = context.UserVideos.Include(uv => uv.Video.Channel).Include(uv => uv.Video.Category).Where(uv => uv.User.Id == userID).Distinct().ProjectTo<UserVideoDataDTO>(mapper.ConfigurationProvider);


        return await PagedList<UserVideoDataDTO>.CreateAsync(query, userVideoParams.PageNumber, userVideoParams.PageSize);
    }
}
