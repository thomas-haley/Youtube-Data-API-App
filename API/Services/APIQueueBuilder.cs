using System;
using API.Interfaces;

namespace API.Services;

public class APIQueueBuilder(IAPIQueue _taskQueue, ILogger<APIQueueBuilder> _logger, IHostApplicationLifetime _lifetime) : IAPIQueueBuilder
{
    private readonly CancellationToken _cancellationToken = _lifetime.ApplicationStopping;

    
    public async ValueTask BuildWorkItem()
    {
        await _taskQueue.QueueBackgroundWorkItemAsync(WorkItem);
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
