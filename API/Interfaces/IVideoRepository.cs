using System;
using API.Entities;

namespace API.Interfaces;

public interface IVideoRepository
{
    void Update(AppVideo user);
    Task<AppVideo?> GetByAPIId(string id);

    bool CreateVideo(AppVideo user);
}
