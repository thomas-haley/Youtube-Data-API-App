using System.Globalization;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class CategoryController(IUnitOfWork unitOfWork, IHTMLFileParser fileParser, IMapper mapper) : BaseAPIController
{
    // [HttpGet()]
}
