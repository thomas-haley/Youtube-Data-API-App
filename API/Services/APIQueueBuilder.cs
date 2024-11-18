using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class APIQueueBuilder(IAPIQueue _taskQueue, ILogger<APIQueueBuilder> _logger, IHostApplicationLifetime _lifetime, IServiceScopeFactory serviceScopeFactory) : IAPIQueueBuilder
{



    private readonly CancellationToken _cancellationToken = _lifetime.ApplicationStopping;

    

    public async ValueTask BuildWorkItem()
    {
        // var test = [];
        // await _taskQueue.QueueBackgroundWorkItemAsync( WorkItem);
    }


    public async ValueTask BuildWorkItemFromDTO(UserQueueDataDTO queueDTO){
        await _taskQueue.QueueBackgroundWorkItemAsync(ct => ParseQueueDTO(ct, queueDTO));
    }

    
    public async ValueTask ParseQueueDTO(CancellationToken token, UserQueueDataDTO queueDTO){
        var guid = Guid.NewGuid().ToString();
        _logger.LogInformation("Queued Background Task {Guid} is starting.", guid);
        while(!token.IsCancellationRequested){
            //Perform logic with the queueDTO here
            _logger.LogInformation($"Request Type: {queueDTO.TaskType}");

            using(var scope = serviceScopeFactory.CreateScope()){
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var task = await _unitOfWork.QueueRepository.GetTaskByID(queueDTO.Id);
                if(task == null) return;
                task.Completed = true;
                task.Queued = false;
                await _unitOfWork.Complete();
                _logger.LogInformation($"{queueDTO.TaskType}");
            }
            
            return;
        }
    }




    public async ValueTask WorkItem(CancellationToken token){
        var guid = Guid.NewGuid().ToString();
        _logger.LogInformation("Queued Background Task {Guid} is starting. Delaying 2s then printing to terminal.", guid);
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), token);

            _logger.LogInformation("Successfully waited 2s, process ending");
            return;
        }

    }
}
