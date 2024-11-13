using System;

namespace API.Interfaces;

public interface IAPIQueueBuilder
{
    public ValueTask BuildWorkItem();

    public ValueTask WorkItem(CancellationToken token);
}
