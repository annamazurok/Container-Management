using Api.Dtos;
using Api.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("roles")]
[ApiController]
public class RoleController(
    IRoleControllerService roleControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles(
        CancellationToken cancellationToken)
    {
        var result = await roleControllerService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{roleId:int}")]
    public async Task<ActionResult<RoleDto>> GetById(
        [FromRoute] int roleId,
        CancellationToken cancellationToken)
    {
        var result = await roleControllerService.GetByIdAsync(roleId, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => Ok(r),
            () => NotFound());
    }
}