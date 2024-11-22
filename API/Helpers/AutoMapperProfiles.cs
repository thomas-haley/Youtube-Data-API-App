using System;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, SessionDTO>();
        CreateMap<AppQueueTask, QueueDTO>();
        CreateMap<AppUser, UserDataDTO>();
        CreateMap<QueueDTO, UserQueueDataDTO>();
        CreateMap<AppQueueTask, UserQueueDataDTO>();
        CreateMap<AppUserVideos, UserVideoDataDTO>();
        CreateMap<AppChannel, ChannelDataDTO>();
        CreateMap<AppVideo, VideoDataDTO>(); //Sets up mapping for nested ChannelDataDTO within VideoDataDTO
        CreateMap<AppCategory, CategoryDataDTO>();
    }
}
