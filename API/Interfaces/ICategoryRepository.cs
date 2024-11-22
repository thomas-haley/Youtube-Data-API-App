using System;
using API.Entities;
using Google.Apis.YouTube.v3.Data;

namespace API.Interfaces;

public interface ICategoryRepository
{
    void Update(AppCategory category);
    Task<AppCategory?> GetByApiIdAsync(string apiID);

    bool CreateCategory(AppCategory category);

    public AppCategory CreateFromVideoItem(Video videoItem);

    public int CountIncompleteCategories();
}
