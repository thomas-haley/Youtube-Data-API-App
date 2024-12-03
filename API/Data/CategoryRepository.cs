using System;
using API.Entities;
using API.Interfaces;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CategoryRepository(DataContext context) : ICategoryRepository
{
    public void Update(AppCategory category){
        context.Entry(category).State = EntityState.Modified;
    }

    public async Task<AppCategory?> GetByApiIdAsync(string apiID){
        return await context.Categories.SingleOrDefaultAsync(category => category.API_Id == apiID);
    }

    public bool CreateCategory(AppCategory category){
        var entity = context.Categories.Add(category);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }

    //Returns number of queued items with missing data to build queue task
    public AppCategory CreateFromVideoItem(Video videoItem){
        var newCat = new AppCategory{
            API_Id = videoItem.Snippet.CategoryId   
        };
        this.CreateCategory(newCat);

        return newCat;
    }

    public int CountIncompleteCategories(){
        return context.Categories.Where(cat => cat.Category == null).Count();
    }


    public async Task<IEnumerable<AppCategory?>> GetAllUserCategoriesAsync(int id)
    {
        return await context.UserVideos.Where(uv => uv.UserId == id).Select(uv => uv.Video.Category).Where(cat => cat != null).Distinct().ToListAsync();
    }
}
