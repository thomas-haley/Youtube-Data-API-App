using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<AppChannel> Channels {get; set;}
    public DbSet<AppCategory> Categories {get; set;}
    public DbSet<AppVideo> Videos {get; set;}
    public DbSet<AppUserVideos> UserVideos {get; set;}
    public DbSet<AppQueueTask> QueueTasks {get; set;}
}