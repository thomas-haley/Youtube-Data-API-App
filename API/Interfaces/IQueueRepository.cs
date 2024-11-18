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
    public List<UserDataDTO>? GetUsersWithTasks();

    public List<UserQueueDataDTO>? GetTasksDataByUserID(int id);
    public List<AppQueueTask>? GetTasksByUserID(int id);

    public Task<UserQueueDataDTO?> GetTaskDataByID(int id);
    public Task<AppQueueTask?> GetTaskByID(int id);

    public List<UserQueueDataDTO> StartTasks(List<AppQueueTask> tasks);
    
}
