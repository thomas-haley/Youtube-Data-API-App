using API.Data;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

/**
    ErrorController contains endpoints that return various http codes to facilitate testing client error handling
*/

public class ErrorController(IYoutubeAPIService youtubeAPIService) : BaseAPIController
{


    //Bad Request
    [HttpGet("400")]
    public ActionResult Test400(){
        return BadRequest("Test endpoint");
    }

    //Unauthorized
    [HttpGet("401")]
    public ActionResult Test401(){
        return Unauthorized("Test endpoint");
    }

    //Unauthorized checking JWT validity
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth(){
        return "Test endpoint";
    }

    //Not Found
    [HttpGet("404")]
    public ActionResult Test404(){
        return NotFound("Test endpoint");
    }

    //Internal Server Error
    [HttpGet("500")]
    public ActionResult Test500(){
        throw new Exception("Test endpoint");
    }


    //Internal Server Error
    [HttpGet("test")]
    public ActionResult TestEndpoint([FromQuery]UserVideoParams userVideoParams){
        // youtubeAPIService.ListVideos([]);
        return Ok();
        // throw new Exception("Test endpoint");
    }

}
