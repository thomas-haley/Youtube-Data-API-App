using System;
using System.Diagnostics;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VideoRepository(DataContext context, IMapper _mapper) : IVideoRepository
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

    public async Task<AppVideo?> GetByIdAsync(int id)
    {
        return await context.Videos.SingleOrDefaultAsync(video => video.Id == id);
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
        
            if(videoItem.Statistics != null) {
                dbVideo.Views = int.Parse(videoItem.Statistics.ViewCount.ToString()!);
            }

            if(videoItem.ContentDetails != null) {
                dbVideo.Duration = videoItem.ContentDetails.Duration;
            }

            dbVideo.Queued = false;
            dbVideo.Retrieved = true;
            dbVideo.DataFetched = new DateTime().ToUniversalTime();
            this.Update(dbVideo);
        }

        return true;
    }


}
