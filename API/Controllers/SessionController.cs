using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace API.Controllers;

[Authorize]
public class SessionController(DataContext context) : BaseAPIController
{

    //Update to get session flags for client
    [HttpGet("flags")]
    public async Task<ActionResult<SessionDTO>> GetSessionFlags(){
        var idString = HttpContext.User.FindFirst("id")?.Value;
        Console.WriteLine(HttpContext.User);
        if(idString == null) return BadRequest("Invalid user id");

        var id = Int32.Parse(idString);

        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if(user == null) return BadRequest("Unable to load user session flags");

        return new SessionDTO{
            AllowUpload = user.AllowUpload,
            DataUploaded = user.DataUploaded
        };
    }


}
