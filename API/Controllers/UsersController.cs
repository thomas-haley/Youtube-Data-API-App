using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IHTMLFileParser fileParser) : BaseAPIController
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
    public ActionResult UploadWatchHistory(WatchUploadDTO fileDTO){

        // Console.WriteLine(fileDTO.File);
        List<HTMLDataDTO> videos = fileParser.ParseHTML(fileDTO.File);
        if(videos.Count != 0){
            Console.WriteLine(videos[1].Href);
        } else {
            Console.WriteLine("No videos found");
        }
        return Ok();
    }
}
