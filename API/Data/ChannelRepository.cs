using API.Entities;
using API.Interfaces;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ChannelRepository(DataContext context) : IChannelRepository
{
    public void Update(AppChannel channel){
        context.Entry(channel).State = EntityState.Modified;
    }
    
    public async Task<bool> CreateChannel(AppChannel channel)
    {
        var entity = await context.Channels.AddAsync(channel);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }

    public async Task<AppChannel?> GetByIdAsync(int id)
    {
        return await context.Channels.SingleOrDefaultAsync(ch => ch.Id == id);
    }

    public async Task<AppChannel?> GetByApiIdAsync(string apiID)
    {
        return await context.Channels.SingleOrDefaultAsync(ch => ch.API_Id == apiID);
    }

    public async Task<bool> UpsertAPIChannelItem(Channel apiChannel)
    {
        AppChannel? dbChannel = await this.GetByApiIdAsync(apiChannel.Id);
        Thumbnail? thumbnail = apiChannel.Snippet.Thumbnails.Maxres ?? apiChannel.Snippet.Thumbnails.High ?? apiChannel.Snippet.Thumbnails.Medium ?? apiChannel.Snippet.Thumbnails.Default__;
        if(dbChannel == null){
            dbChannel = new AppChannel{
                API_Id = apiChannel.Id,
                Title = apiChannel.Snippet.Title

            };

            if(thumbnail != null){
                dbChannel.Thumbnail = thumbnail.Url;
            }

            await this.CreateChannel(dbChannel);
        } else {
            if(thumbnail != null){
                dbChannel.Thumbnail = thumbnail.Url;
            }
        }

        return true;
    }


    public async Task<IEnumerable<AppChannel?>> GetAllUserChannelsAsync(int id)
    {
        return await context.UserVideos.Where(uv => uv.UserId == id).Select(uv => uv.Video.Channel).Distinct().ToListAsync();
    }
}