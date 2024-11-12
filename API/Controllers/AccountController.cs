using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext context, ITokenService tokenService) : BaseAPIController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username taken");

        using var hmac = new HMACSHA512();
        
        var user = new AppUser{
            Username = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDTO{
            Username = user.Username,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
       var user = await context.Users.FirstOrDefaultAsync(user => user.Username == loginDTO.Username.ToLower());

       if (user == null) return BadRequest("Invalid User");
       using var hmac = new HMACSHA512(user.PasswordSalt);
       var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

       for (int i = 0; i < computedHash.Length; i++){
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
       }

        return new UserDTO{
            Username = user.Username,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
    }

}
