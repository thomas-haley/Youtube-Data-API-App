using System.ComponentModel.DataAnnotations;
using API.Entities;
using API.Interfaces;

namespace API.DTOs;

public class UserDataDTO
{
    public int Id { get; set; }
    public required string Username { get; set; }

}
