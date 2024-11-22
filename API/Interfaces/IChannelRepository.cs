using System;
using API.DTOs;
using API.Entities;
using Google.Apis.YouTube.v3.Data;

namespace API.Interfaces;

public interface IChannelRepository
{

    public void Update(AppChannel channel);

    public Task<bool> CreateChannel(AppChannel channel);
    public Task<AppChannel?> GetByIdAsync(int id);
    public Task<AppChannel?> GetByApiIdAsync(string apiID);

    public Task<bool> UpsertAPIChannelItem(Channel apiChannel);
    
}
