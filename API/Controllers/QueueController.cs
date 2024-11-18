using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class QueueController(IUnitOfWork unitOfWork, IAPIQueueBuilder queueBuilder, IMapper mapper) : BaseAPIController
{

    //Update to get session flags for client
    [HttpGet("add")]
    public async Task<ActionResult> AddToQueue(){
        await queueBuilder.BuildWorkItem();
        return Ok();
    }


    [AllowAnonymous]
    [HttpGet("user-tasks")]
    public ActionResult<List<UserDataDTO>?> GetUsersWithTasks(){
        return unitOfWork.QueueRepository.GetUsersWithTasks();
    }




    [AllowAnonymous]
    [HttpGet("user-tasks/{id:int}")]
    public ActionResult<List<UserQueueDataDTO>?> GetTasksForUserID(int id){
        return unitOfWork.QueueRepository.GetTasksDataByUserID(id);
    }

    [AllowAnonymous]
    [HttpGet("user-tasks/{id:int}/start")]
    public async Task<ActionResult> TriggerTasksByUserID(int id){

        var tasks = unitOfWork.QueueRepository.GetTasksByUserID(id);
        if(tasks == null) return BadRequest();

        List<UserQueueDataDTO> taskDTOs = unitOfWork.QueueRepository.StartTasks(tasks);

        if(taskDTOs.Count == 0) return BadRequest("Unable to start queue");

        await unitOfWork.Complete();
        foreach(UserQueueDataDTO dto in taskDTOs){
            await queueBuilder.BuildWorkItemFromDTO(dto);
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("tasks/{id:int}")]
    public async Task<ActionResult<UserQueueDataDTO?>> GetTaskDataByID(int id){
        return await unitOfWork.QueueRepository.GetTaskDataByID(id);
    }


    

}
