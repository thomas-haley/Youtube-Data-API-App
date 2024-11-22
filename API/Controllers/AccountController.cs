using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(IUnitOfWork unitOfWork, ITokenService tokenService) : BaseAPIController
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
        var userCreated = unitOfWork.UserRepository.CreateUser(user);
        
        if(!await unitOfWork.Complete()) throw new Exception("Unable to create user at this time");

        return new UserDTO{
            Username = user.Username,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(loginDTO.Username);
    
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
        return await unitOfWork.UserRepository.GetUserByUsernameAsync(username) != null;
        
    }

}
