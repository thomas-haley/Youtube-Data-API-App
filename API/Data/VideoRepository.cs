using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VideoRepository(DataContext context) : IVideoRepository
{
    public void Update(AppVideo video)
    {
        context.Entry(video).State = EntityState.Modified;
    }
    public bool CreateVideo(AppVideo video){
        var entity = context.Videos.Add(video);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }

    public async Task<AppVideo?> GetByApiIdAsync(string apiID)
    {
        return await context.Videos.SingleOrDefaultAsync(video => video.API_Id == apiID);
    }

    public async Task<bool> UpsertAPIVideoItem(Video videoItem, AppCategory dbCategory){
        AppVideo? dbVideo = await this.GetByApiIdAsync(videoItem.Id);
        if(dbVideo == null){
            //Logic to insert db video from video item
        } else {
            dbVideo.Published = DateTime.Parse(videoItem.Snippet.PublishedAtDateTimeOffset.ToString()!);
            dbVideo.Category = dbCategory;
            IList<string>? categories = videoItem.TopicDetails?.TopicCategories;
            if(categories != null){
                dbVideo.Topics = categories.ToList();
            }
            Thumbnail? thumbnail = videoItem.Snippet.Thumbnails.Maxres ?? videoItem.Snippet.Thumbnails.High ?? videoItem.Snippet.Thumbnails.Medium ?? videoItem.Snippet.Thumbnails.Standard;
            if(thumbnail != null){
                dbVideo.Thumbnail = thumbnail.Url;
            }
            dbVideo.Queued = false;
            dbVideo.Retrieved = true;
            this.Update(dbVideo);
        }

        return true;
    }

    public async Task<List<int>> GetUserVideosAsync(int userID)
    {
        return await context.UserVideos.Where(uv => uv.User.Id == userID).Select(uv => uv.Video.Id).ToListAsync();
    }
}
