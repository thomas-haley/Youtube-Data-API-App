using System;
using API.DTOs;
using API.Entities;
using Google.Apis.YouTube.v3.Data;

namespace API.Interfaces;

public interface IVideoRepository
{
    void Update(AppVideo video);
    Task<AppVideo?> GetByApiIdAsync(string apiID);
    Task<AppVideo?> GetByIdAsync(int id);

    bool CreateVideo(AppVideo user);

    Task<bool> UpsertAPIVideoItem(Video videoItem, AppCategory dbCategory);


    

}
