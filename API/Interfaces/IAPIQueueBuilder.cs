using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IAPIQueueBuilder
{

    public ValueTask BuildWorkItemFromDTO(UserQueueDataDTO queueDTO);
}
