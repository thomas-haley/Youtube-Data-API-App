using API.DTOs;
using API.Entities;
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
        Console.WriteLine(parsedData.ToList().Count);
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
        Console.WriteLine($"Batching {queueDTO.Channels.Count} channels and {queueDTO.Videos.Count} videos");
        var batchedTasks = unitOfWork.QueueRepository.BatchTasks(queueDTO);

        if(RequireAdminToQueue){
            unitOfWork.QueueRepository.CreateTaskFromDTOs(batchedTasks);
        } else {
            foreach(var dto in batchedTasks){
                await queueBuilder.BuildWorkItemFromDTO(mapper.Map<UserQueueDataDTO>(dto));
            }
        }

        if(!await unitOfWork.Complete()) throw new Exception("Unable to save batched tasks for queue");
        

    //    Console.WriteLine(parsedData[15].Title);
        return Ok();
    }
}
