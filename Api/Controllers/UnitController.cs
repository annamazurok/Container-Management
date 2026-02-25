using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Units.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("units")]
[Authorize]
[ApiController]
public class UnitController(
    ISender sender,
    IUnitControllerService unitControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UnitDto>>> GetUnits(CancellationToken cancellationToken)
    {
        var result = await unitControllerService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("unitId:int")]
    public async Task<ActionResult<UnitDto>> GetUnitId(
        [FromRoute] int unitId, 
        CancellationToken cancellationToken)
    {
        var result = await unitControllerService.GetByIdAsync(unitId, cancellationToken);
        
        return result.Match<ActionResult<UnitDto>>(
            u => Ok(u),
            () => NotFound());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<UnitDto>> CreateUnit(
        [FromBody] CreateUnitDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateUnitCommand
        {
            Title = request.Title,
            UnitType = request.UnitType
        };
        
        var newUnit = await sender.Send(input, cancellationToken);
        
        return newUnit.Match<ActionResult<UnitDto>>(
            u => UnitDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<UnitDto>> UpdateUnit(
        [FromBody] UnitDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateUnitCommand
        {
            Id = request.Id,
            Title = request.Title,
            UnitType = request.UnitType
        };
        
        var updatedUnit = await sender.Send(input, cancellationToken);
        
        return updatedUnit.Match<ActionResult<UnitDto>>( 
            u => UnitDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<UnitDto>> DeleteUnit(
        [FromRoute] int unitId,
        CancellationToken cancellationToken)
    {
        var input = new DeleteUnitCommand
        {
            Id = unitId
        };
        
        var deletedUnit = await sender.Send(input, cancellationToken);
        
        return deletedUnit.Match<ActionResult<UnitDto>>(
            u => UnitDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
}