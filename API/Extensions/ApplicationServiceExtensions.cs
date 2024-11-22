using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt => {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserVideoRepository, UserVideoRepository>();
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IYoutubeAPIService, YoutubeAPIService>();
        services.AddScoped<IHTMLFileParser, HTMLFileParser>();

        //API Task queue
        services.AddHostedService<APIQueueService>();
        services.AddScoped<IAPIQueueBuilder, APIQueueBuilder>();

        services.AddSingleton<IAPIQueue>(
            ctx => 
            {
                if(!int.TryParse(config["QueueCapacity"], out var queueCapacity))
                    queueCapacity = 10;
                return new APIQueue(queueCapacity);
            }
        );

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
