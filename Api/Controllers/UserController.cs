using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UserController(
    ISender sender,
    IUserControllerService userControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers(
        CancellationToken cancellationToken)
    {
        var result = await userControllerService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetById(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var result = await userControllerService.GetByIdAsync(userId, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(u),
            () => NotFound());
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(
        [FromRoute] string email,
        CancellationToken cancellationToken)
    {
        var result = await userControllerService.GetByEmailAsync(email, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(u),
            () => NotFound());
    }

    [HttpGet("role/{roleId:int}")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetByRole(
        [FromRoute] int roleId,
        CancellationToken cancellationToken)
    {
        var result = await userControllerService.GetByRoleIdAsync(roleId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody] CreateUserDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand
        {
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            FathersName = request.FathersName,
            RoleId = request.RoleId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser(
        [FromBody] UserDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand
        {
            Id = request.Id,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            FathersName = request.FathersName,
            RoleId = request.RoleId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult<UserDto>> DeleteUser(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand
        {
            Id = userId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => Ok(UserDto.FromDomainModel(u)),
            e => e.ToObjectResult());
    }
}