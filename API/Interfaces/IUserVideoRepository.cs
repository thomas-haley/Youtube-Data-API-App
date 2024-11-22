using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserVideoRepository
{

    public Task<VideoAddedDTO> CreateVideoReferencesAsync(AppUser user, VideoDTO videos);
    void Update(AppUserVideos user);

    bool CreateUserVideo(AppUserVideos user);
}
