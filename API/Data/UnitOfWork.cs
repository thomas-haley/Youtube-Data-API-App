using System;
using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, IUserRepository userRepository, IUserVideoRepository userVideoRepository, IQueueRepository queueRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IUserVideoRepository UserVideoRepository => userVideoRepository;
    // public IVideoRepository VideoRepository => videoRepository;
    public IQueueRepository QueueRepository => queueRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
