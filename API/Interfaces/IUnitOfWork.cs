using System;

namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository {get;}
    IUserVideoRepository UserVideoRepository {get;}
    IQueueRepository QueueRepository {get;}
    Task<bool> Complete();
    bool HasChanges();
}
