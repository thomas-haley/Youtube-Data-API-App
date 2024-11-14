using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IUserVideoRepository userVideoRepository, IHTMLFileParser fileParser) : BaseAPIController
{


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id){
        var user = await userRepository.GetUserByIdAsync(id);

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
        var user = await userRepository.GetUserByIdAsync(id);
        if(user == null) return BadRequest("Invalid user");
        Console.WriteLine(parsedData.ToList().Count);
        QueueDTO queueDTO = new QueueDTO{
            User = user,
            TaskType = "videos/list",
            Videos = [],
            Channels = []
        };

        foreach(var video in parsedData){
            VideoAddedDTO addedDTO = await userVideoRepository.CreateVideoReferencesAsync(user, video);
            //TODO: Add logic to queue items into batches
            if(addedDTO.NewVideo){
                queueDTO.Videos.Add(video.API_Id);
            }

            if(addedDTO.NewVideo){
                queueDTO.Videos.Add(video.API_Id);
            }
        }

        Console.WriteLine(queueDTO.Videos.Count);
        Console.WriteLine(queueDTO.Channels.Count);
       Console.WriteLine(parsedData[15].Title);
        return Ok();
    }
}
