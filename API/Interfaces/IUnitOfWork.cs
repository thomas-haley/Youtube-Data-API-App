using System;

namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository {get;}
    IUserVideoRepository UserVideoRepository {get;}
    IQueueRepository QueueRepository {get;}
    IVideoRepository VideoRepository {get;}
    ICategoryRepository CategoryRepository {get;}
    IChannelRepository ChannelRepository {get;}
    Task<bool> Complete();
    bool HasChanges();
}
