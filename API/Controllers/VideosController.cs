using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class VideosController(IUnitOfWork unitOfWork, IMapper mapper) : BaseAPIController
{


    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VideoDataDTO>> GetVideoData(int id){
        //Build out logic to build VideoDataDTO with a nested ChannelDataDTO and return, 
        //will need to expand AppVideoEntity and re-fetch api data
        AppVideo? video = await unitOfWork.VideoRepository.GetByIdAsync(id);
        if(video == null) return NotFound();

        //Next populate channel data within AppVideo because SQLite sucks
        if(video.ChannelId != null)
        {
            video.Channel = await unitOfWork.ChannelRepository.GetByIdAsync((int)video.ChannelId);
        }

        if(video.CategoryId != null)
        {
            video.Category = await unitOfWork.CategoryRepository.GetByApiIdAsync(video.CategoryId.ToString()!);
        }

        return mapper.Map<VideoDataDTO>(video);
    } 

}
