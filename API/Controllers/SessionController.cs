using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class SessionController(IUserRepository userRepository, IMapper mapper) : BaseAPIController
{

    //Update to get session flags for client
    [HttpGet("flags")]
    public async Task<ActionResult<SessionDTO>> GetSessionFlags(){
        var idString = HttpContext.User.FindFirst("id")?.Value;
        Console.WriteLine(HttpContext.User);
        if(idString == null) return BadRequest("Invalid user id");

        var id = Int32.Parse(idString);

        var user = await userRepository.GetUserByIdAsync(id);

        if(user == null) return BadRequest("Unable to load user session flags");
        return mapper.Map<SessionDTO>(user);
    }


}
