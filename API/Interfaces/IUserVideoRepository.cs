using System;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserVideoRepository
{

    public Task<VideoAddedDTO> CreateVideoReferencesAsync(AppUser user, VideoDTO videos);
    void Update(AppUserVideos user);

    bool CreateUserVideo(AppUserVideos user);

    Task<PagedList<UserVideoDataDTO>> GetUserVideosAsync(UserVideoParams userVideoParams, int userID, UserVideoFilters? userVideoFilters);
    public IQueryable<AppUserVideos> FilterQuery(IQueryable<AppUserVideos> query, UserVideoFilters videoFilters);
}
