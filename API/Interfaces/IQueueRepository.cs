using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IQueueRepository
{

    public void CreateTaskFromDTOs(List<QueueDTO> queueItems);   
    void Update(AppQueueTask queueTask);

    public Task<bool> CreateQueueTask(AppQueueTask queueTask);

    public List<QueueDTO> BatchTasks(QueueDTO queueDTO);


    //Admin task loading logic
    public List<UserDataDTO>? GetUsersWithTasksToQueue();

    public List<UserQueueDataDTO>? GetTasksDataByUserID(int id, bool includeCanceled = false, bool includeComplete = false);
    public List<AppQueueTask>? GetTasksByUserID(int id, bool includeCanceled = false, bool includeComplete = false);

    public Task<UserQueueDataDTO?> GetTaskDataByID(int id);
    public Task<AppQueueTask?> GetTaskByID(int id);

    public List<UserQueueDataDTO> StartTasks(List<AppQueueTask> tasks);

    public void CancelTasks(List<AppQueueTask> tasks);
    
}
