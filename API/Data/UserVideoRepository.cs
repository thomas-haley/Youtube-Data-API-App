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

        public async Task<PagedList<UserVideoDataDTO>> GetUserVideosAsync(UserVideoParams userVideoParams, int userID, UserVideoFilters? userVideoFilters)
    {   
        var query = context.UserVideos.Include(uv => uv.Video.Channel).Include(uv => uv.Video.Category).Where(uv => uv.User.Id == userID);

        if(userVideoFilters != null)
        {
            query = this.FilterQuery(query, userVideoFilters);
        }

        var projectedQuery = query.Distinct().ProjectTo<UserVideoDataDTO>(mapper.ConfigurationProvider);

        return await PagedList<UserVideoDataDTO>.CreateAsync(projectedQuery, userVideoParams.PageNumber, userVideoParams.PageSize);
    }


    public IQueryable<AppUserVideos> FilterQuery(IQueryable<AppUserVideos> query, UserVideoFilters videoFilters)
    {
        if(videoFilters.Channel != null)
        {
            query = query.Where<AppUserVideos>(uv => uv.Video.Channel!.Id == videoFilters.Channel);
        }

        if(videoFilters.DateWatchedStart != null)
        {
            var dws = DateTime.Parse(videoFilters.DateWatchedStart);
            query = query.Where<AppUserVideos>(uv => uv.Watched >= dws);
        }

        if(videoFilters.DateWatchedEnd != null)
        {
            var dwe = DateTime.Parse(videoFilters.DateWatchedEnd);
            query = query.Where<AppUserVideos>(uv => uv.Watched <= dwe);
        }

        if(videoFilters.PublishedStart != null)
        {
            var ps = DateTime.Parse(videoFilters.PublishedStart);
            query = query.Where<AppUserVideos>(uv => uv.Video.Published >= ps);
        }

        if(videoFilters.PublishedEnd != null)
        {
            var pe = DateTime.Parse(videoFilters.PublishedEnd);
            query = query.Where<AppUserVideos>(uv => uv.Video.Published <= pe);
        }

        //TODO: Add duration filter here once you figure out how you want to split the data (Maybe separate days, hours, minutes, seconds cols?)

        if(videoFilters.Views != null)
        {
            var viewsSplit = videoFilters.Views.Split("_");
            var filterViews = int.Parse(viewsSplit[1]);
            switch(viewsSplit[0]) // Switch on operator of videoFilters.Views
            {
                case "<":
                    query = query.Where<AppUserVideos>(uv => uv.Video.Views == null ? false : uv.Video.Views < filterViews);
                    break;
                case ">":
                    query = query.Where<AppUserVideos>(uv => uv.Video.Views == null ? false : uv.Video.Views > filterViews);
                    break;
                case "<=":
                    query = query.Where<AppUserVideos>(uv => uv.Video.Views == null ? false : uv.Video.Views <= filterViews);
                    break;
                case ">=":
                    query = query.Where<AppUserVideos>(uv => uv.Video.Views == null ? false : uv.Video.Views >= filterViews);
                    break;
                case "=":
                    query = query.Where<AppUserVideos>(uv => uv.Video.Views == null ? false : uv.Video.Views == filterViews);
                    break;
                default: //Default skips
                    break;

            }
        }

        if(videoFilters.Category != null)
        {
            query = query.Where<AppUserVideos>(uv => uv.Video.CategoryId == videoFilters.Category);
        }

        return query;
    }
}
