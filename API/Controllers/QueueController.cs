using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class QueueController(IAPIQueueBuilder queueBuilder) : BaseAPIController
{

    //Update to get session flags for client
    [HttpGet("add")]
    public async Task<ActionResult> AddToQueue(){
        await queueBuilder.BuildWorkItem();
        return Ok();
    }


}
