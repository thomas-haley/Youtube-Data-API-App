using System;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces;

public class QueueRepository(DataContext context, IMapper mapper) : IQueueRepository
{

    
    public const int MaxBatchSizeVideo = 50;
    public const int MaxBatchSizeChannel = 5;

    public async void CreateTaskFromDTOs(List<QueueDTO> queueItems){
        foreach(QueueDTO queueItem in queueItems){
            AppQueueTask queueTask = new AppQueueTask{
                User = queueItem.User,
                TaskType = queueItem.TaskType,
                Videos = queueItem.Videos,
                Channels = queueItem.Channels,
                Categories = queueItem.Categories  
            };

            var created = await this.CreateQueueTask(queueTask);
        }
    }
    public void Update(AppQueueTask queueTask){
        context.Entry(queueTask).State = EntityState.Modified;
    }

    public async Task<bool> CreateQueueTask(AppQueueTask queueTask){
        var entity = await context.QueueTasks.AddAsync(queueTask);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }


    public List<QueueDTO> BatchTasks(QueueDTO queueDTO){
        AppUser user = queueDTO.User;
        if(user == null) throw new Exception("Cannot batch for null user");
        List<string>? videos = queueDTO.Videos;
        var channels = queueDTO.Channels;
        List<QueueDTO> batchedDTOs = [];
        //Extract videos/channels from single dto, break apart into lists of MaxBatchSize for given data
        if(videos != null){
            var batchedVideos = videos.Select((v, i) => new {Index = i, Value = v})
                                    .GroupBy(vid => vid.Index / MaxBatchSizeVideo)
                                    .Select(vid => vid.Select(v => v.Value).ToList())
                                    .ToList();
            

            foreach(var batch in batchedVideos){
                batchedDTOs.Add(new QueueDTO{
                    User = user,
                    TaskType = "videos/list",
                    Videos = batch
                });
            }

          

        }

        if(channels != null){
            var batchedChannels = channels.Select((v, i) => new {Index = i, Value = v})
                                    .GroupBy(chan => chan.Index / MaxBatchSizeChannel)
                                    .Select(l => l.Select(chan => chan.Value).ToList())
                                    .ToList();
            

            foreach(var batch in batchedChannels){
                batchedDTOs.Add(new QueueDTO{
                    User = user,
                    TaskType = "channels/list",
                    Channels = batch
                });
            }


        }
        return batchedDTOs;
    }


    public List<UserDataDTO>? GetUsersWithTasksToQueue(){
        var usersWithTasks = context.QueueTasks.Where(qt => !qt.Completed && !qt.Canceled && !qt.Queued).Select(qt => qt.User).Distinct().ToList();
        if (usersWithTasks == null) return [];

        List<UserDataDTO> output = [];
        foreach(var user in usersWithTasks){
            output.Add(mapper.Map<UserDataDTO>(user));
        }

        return output;
    }


    public List<UserQueueDataDTO>? GetTasksDataByUserID(int id, bool includeCanceled = false, bool includeComplete = false){
        var queueTasks = context.QueueTasks.Where(qt => qt.User.Id == id);
        if(!includeCanceled){
            queueTasks = queueTasks = queueTasks.Where(qt => !qt.Canceled);

        }
        if(queueTasks == null) return null;

        if(!includeComplete){
            queueTasks = queueTasks = queueTasks.Where(qt => !qt.Completed);

        }
        if(queueTasks == null) return null;

        return [.. queueTasks.Select(queueTask =>     new UserQueueDataDTO{
            Id = queueTask.Id,
            User = mapper.Map<UserDataDTO>(queueTask.User),
            TaskType = queueTask.TaskType,
            Videos = queueTask.Videos,
            Channels = queueTask.Channels,
            Categories = queueTask.Categories,
            Canceled = queueTask.Canceled,
            Completed = queueTask.Completed
        })];

    }

    public List<AppQueueTask>? GetTasksByUserID(int id, bool includeCanceled = false, bool includeComplete = false){
        var queueTasks = context.QueueTasks.Where(qt => qt.User.Id == id);
        if(!includeCanceled){
            queueTasks = queueTasks = queueTasks.Where(qt => !qt.Canceled);

        }
        if(queueTasks == null) return null;

        if(!includeComplete){
            queueTasks = queueTasks = queueTasks.Where(qt => !qt.Completed);

        }

        return queueTasks.ToList();

    }

 
    public async Task<UserQueueDataDTO?> GetTaskDataByID(int id){
        var queueTask = await context.QueueTasks.SingleOrDefaultAsync(qt => qt.Id == id);

        if (queueTask == null) return null;

        return new UserQueueDataDTO{
            Id = queueTask.Id,
            User = mapper.Map<UserDataDTO>(queueTask.User),
            TaskType = queueTask.TaskType,
            Videos = queueTask.Videos,
            Channels = queueTask.Channels,
            Categories = queueTask.Categories,
            Canceled = queueTask.Canceled,
            Completed = queueTask.Completed
        };

        
    }

    public async Task<AppQueueTask?> GetTaskByID(int id){
        var queueTask = await context.QueueTasks.SingleOrDefaultAsync(qt => qt.Id == id);

        if(queueTask == null) return null;

        return queueTask;    
    }



    public List<UserQueueDataDTO> StartTasks(List<AppQueueTask> tasks){
        List<UserQueueDataDTO> taskDTOs = [];
        foreach(AppQueueTask task in tasks){
            task.Canceled = false;
            task.Completed = false;
            task.Queued = true;
            taskDTOs.Add(mapper.Map<UserQueueDataDTO>(task));
        }
        return taskDTOs;
    }

    public void CancelTasks(List<AppQueueTask> tasks){
        foreach(AppQueueTask task in tasks){
            task.Canceled = true;
            task.Completed = false;
            task.Queued = false;
        }
    }
}
