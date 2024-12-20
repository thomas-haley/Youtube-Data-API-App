using System.Globalization;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUnitOfWork unitOfWork, IHTMLFileParser fileParser, IAPIQueueBuilder queueBuilder, IMapper mapper) : BaseAPIController
{
    public bool RequireAdminToQueue = true;

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id){
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(id);

        if(user == null) return NotFound();
 

        return user;
    }

    [Authorize]
    [HttpPost("upload-watch-history")]
    public async Task<ActionResult> UploadWatchHistory(WatchUploadDTO fileDTO){

        var parsedData = fileParser.ParseHTML(fileDTO.File);
        var idString = HttpContext.User.FindFirst("id")?.Value;
        if(idString == null) return BadRequest("Invalid user id");
        var id = Int32.Parse(idString);
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(id);
        if(user == null) return BadRequest("Invalid user");
        QueueDTO queueDTO = new QueueDTO{
            User = user,
            TaskType = "videos/list",
            Videos = [],
            Channels = []
        };

        foreach(var video in parsedData){
            VideoAddedDTO addedDTO = await unitOfWork.UserVideoRepository.CreateVideoReferencesAsync(user, video);
            //TODO: Add logic to queue items into batches
            if(addedDTO.NewVideo){
                queueDTO.Videos.Add(video.API_Id);
            }

            if(addedDTO.NewChannel){
                queueDTO.Channels.Add(video.ChannelId);
            }

        }
        var batchedTasks = unitOfWork.QueueRepository.BatchTasks(queueDTO);

        if(RequireAdminToQueue){
            unitOfWork.QueueRepository.CreateTaskFromDTOs(batchedTasks);
        } else {
            foreach(var dto in batchedTasks){
                await queueBuilder.BuildWorkItemFromDTO(mapper.Map<UserQueueDataDTO>(dto));
            }
        }

        user.DataUploaded = true;
        unitOfWork.UserRepository.Update(user);
        if(!await unitOfWork.Complete()) throw new Exception("Unable to save batched tasks for queue");
        
        return Ok();
    }





    [AllowAnonymous]
    [HttpGet("{id:int}/videos")]
    public async Task<ActionResult<IEnumerable<UserVideoDataDTO>>> GetUsersVideos([FromQuery]UserVideoParams userVideoParams, [FromQuery] UserVideoFilters videoFilters,  int id)
    {
        var userVideos =  await unitOfWork.UserVideoRepository.GetUserVideosAsync(userVideoParams, id, videoFilters);
        Response.AddPaginationHeader(userVideos);
        return Ok(userVideos);
    }


    [HttpGet("channels/all")]
    public async Task<ActionResult<IEnumerable<ChannelDataDTO>>> GetAllUserChannels()
    {
        var idString = HttpContext.User.FindFirst("id")?.Value;
        if(idString == null) return BadRequest("Invalid user id");
        var userVideos =  await unitOfWork.ChannelRepository.GetAllUserChannelsAsync(Int32.Parse(idString));
        return Ok(userVideos);
    }



    [HttpGet("categories/all")]
    public async Task<ActionResult<IEnumerable<ChannelDataDTO>>> GetAllUserCategories()
    {
        var idString = HttpContext.User.FindFirst("id")?.Value;
        if(idString == null) return BadRequest("Invalid user id");
        var userVideos =  await unitOfWork.CategoryRepository.GetAllUserCategoriesAsync(Int32.Parse(idString));
        return Ok(userVideos);
    }
}
