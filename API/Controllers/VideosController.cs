using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class VideosController(IUnitOfWork unitOfWork) : BaseAPIController
{


    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetVideoData(int id){
        //Build out logic to build VideoDataDTO with a nested ChannelDataDTO and return, will need to expand AppVideoEntity and re-fetch api data

        return Ok();
    } 

}
