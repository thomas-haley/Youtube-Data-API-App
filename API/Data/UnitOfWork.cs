using System;
using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, IUserRepository userRepository, IUserVideoRepository userVideoRepository, IQueueRepository queueRepository, IVideoRepository videoRepository, ICategoryRepository categoryRepository, IChannelRepository channelRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IUserVideoRepository UserVideoRepository => userVideoRepository;
    // public IVideoRepository VideoRepository => videoRepository;
    public IQueueRepository QueueRepository => queueRepository;
    public IVideoRepository VideoRepository => videoRepository;
    public ICategoryRepository CategoryRepository => categoryRepository;
    public IChannelRepository ChannelRepository => channelRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
