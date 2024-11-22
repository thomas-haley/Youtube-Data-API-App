using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class APIQueueBuilder(IAPIQueue _taskQueue, ILogger<APIQueueBuilder> _logger, IHostApplicationLifetime _lifetime, IServiceScopeFactory serviceScopeFactory) : IAPIQueueBuilder
{



    private readonly CancellationToken _cancellationToken = _lifetime.ApplicationStopping;



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
                var _youtubeService = scope.ServiceProvider.GetRequiredService<IYoutubeAPIService>();
                var task = await _unitOfWork.QueueRepository.GetTaskByID(queueDTO.Id);
                if(task == null) return;

                bool taskCompleted = false;
                switch(task.TaskType){
                    case "videos/list":
                        taskCompleted = await this.handleVideosList(queueDTO);
                        break;
                    case "channels/list":
                        taskCompleted = await this.handleChannelsList(queueDTO);
                        break;
                    default:
                        break;
                }

                task.Completed = taskCompleted;
                task.Queued = false;
                await _unitOfWork.Complete();
            }
            
            return;
        }
    }

    private async Task<bool> handleVideosList(UserQueueDataDTO queueDTO){
        VideoListResponse? listResults = null;
        using(var scope = serviceScopeFactory.CreateScope())
        {
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var _youtubeService = scope.ServiceProvider.GetRequiredService<IYoutubeAPIService>();
            if(queueDTO.Videos!.Count == 1){
            //Do single video request here
            } else {
                listResults = _youtubeService.ListVideos(queueDTO.Videos);
            }

            if(listResults == null) return false;
            if(listResults.Items.Count == 0) return false;

            //Loop results, insert categories first, then update videos
            foreach(Video video in listResults.Items){
                AppCategory? category = await _unitOfWork.CategoryRepository.GetByApiIdAsync(video.Snippet.CategoryId);    
                if(category == null){
                    category = _unitOfWork.CategoryRepository.CreateFromVideoItem(video);
                    //Ensure new category saved to DB before references are made to it
                }
                await _unitOfWork.VideoRepository.UpsertAPIVideoItem(video, category);
                await _unitOfWork.Complete();
            }
            return true;
        }
        
    } 

    private async Task<bool> handleChannelsList(UserQueueDataDTO queueDTO){
        ChannelListResponse? listResults = null;
        using(var scope = serviceScopeFactory.CreateScope())
        {
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var _youtubeService = scope.ServiceProvider.GetRequiredService<IYoutubeAPIService>();
            if(queueDTO.Channels!.Count == 1){
            //Do single channel request here
            } else {
                listResults = _youtubeService.ListChannels(queueDTO.Channels);
            }

            if(listResults == null) return false;
            if(listResults.Items.Count == 0) return false;

            //Loop results, insert categories first, then update videos
            foreach(Channel channel in listResults.Items){
                await _unitOfWork.ChannelRepository.UpsertAPIChannelItem(channel);
                await _unitOfWork.Complete();
            }
            return true;
        }
    }
}
