using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class QueueController(IUnitOfWork unitOfWork, IAPIQueueBuilder queueBuilder, IMapper mapper) : BaseAPIController
{


    [AllowAnonymous]
    [HttpGet("user-tasks")]
    public ActionResult<List<UserDataDTO>?> GetUsersWithTasks(){
        return unitOfWork.QueueRepository.GetUsersWithTasksToQueue();
    }




    [AllowAnonymous]
    [HttpGet("user-tasks/{id:int}")]
    public ActionResult<List<UserQueueDataDTO>?> GetTasksForUserID(int id){
        return unitOfWork.QueueRepository.GetTasksDataByUserID(id, false, false);
    }

    [AllowAnonymous]
    [HttpPost("user-tasks/{id}/start")]
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
    [HttpPost("user-tasks/{id}/cancel")]
    public async Task<ActionResult> CancelTasksByUserID(int id){

        var tasks = unitOfWork.QueueRepository.GetTasksByUserID(id);
        if(tasks == null) return BadRequest();

        unitOfWork.QueueRepository.CancelTasks(tasks);

        await unitOfWork.Complete();
        
        return Ok();
    }


    [AllowAnonymous]
    [HttpPost("tasks/{id}/start")]
    public async Task<ActionResult> TriggerTaskByID(int id){
        var task = await unitOfWork.QueueRepository.GetTaskByID(id);
        List<UserQueueDataDTO> taskDTO = unitOfWork.QueueRepository.StartTasks([task]);
        if(taskDTO.Count != 1) return BadRequest();
        await unitOfWork.Complete();

        await queueBuilder.BuildWorkItemFromDTO(taskDTO.First<UserQueueDataDTO>());
        return Ok();
    }


    [AllowAnonymous]
    [HttpPost("tasks/{id}/cancel")]
    public async Task<ActionResult> CancelTaskByID(int id){
        var task = await unitOfWork.QueueRepository.GetTaskByID(id);
        unitOfWork.QueueRepository.CancelTasks([task]);
        await unitOfWork.Complete();

        return Ok();
    }











    [AllowAnonymous]
    [HttpGet("tasks/{id:int}")]
    public async Task<ActionResult<UserQueueDataDTO?>> GetTaskDataByID(int id){
        return await unitOfWork.QueueRepository.GetTaskDataByID(id);
    }





    

}
