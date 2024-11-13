using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        username = username.ToLower();
        return await context.Users.SingleOrDefaultAsync(user => user.Username == username);
    }
    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public bool CreateUser(AppUser user){
        var entity = context.Users.Add(user);
        if(entity.IsKeySet){
            return true;
        }
        return false;
    }
}
