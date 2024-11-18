using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IAPIQueueBuilder
{
    public ValueTask BuildWorkItem();

    public ValueTask BuildWorkItemFromDTO(UserQueueDataDTO queueDTO);
    public ValueTask WorkItem(CancellationToken token);
}
