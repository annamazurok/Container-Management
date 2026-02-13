using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.ContainerTypes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("container-types")]
[ApiController]
public class ContainerTypeController(
    ISender sender,
    IContainerTypeControllerService containerTypeControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<ContainerTypeDto>> GetContainerTypes(
        CancellationToken cancellationToken)
    {
        return await containerTypeControllerService.GetAllAsync(cancellationToken);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContainerTypeDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await containerTypeControllerService.GetByIdAsync(id, cancellationToken);

        return result.Match<ActionResult<ContainerTypeDto>>(
            ct => Ok(ct),
            () => NotFound());
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ContainerTypeDto>> GetByName(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var result = await containerTypeControllerService.GetByNameAsync(name, cancellationToken);

        return result.Match<ActionResult<ContainerTypeDto>>(
            ct => Ok(ct),
            () => NotFound());
    }

    [HttpGet("volume")]
    public async Task<ActionResult<IReadOnlyList<ContainerTypeDto>>> GetByVolumeRange(
        [FromQuery] int minVolume,
        [FromQuery] int maxVolume,
        CancellationToken cancellationToken)
    {
        var result = await containerTypeControllerService.GetByVolumeRangeAsync(minVolume, maxVolume, cancellationToken);
        return Ok(result);
    }

    [HttpGet("unit/{unitId:int}")]
    public async Task<ActionResult<IReadOnlyList<ContainerTypeDto>>> GetByUnit(
        [FromRoute] int unitId,
        CancellationToken cancellationToken)
    {
        var result = await containerTypeControllerService.GetByUnitIdAsync(unitId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ContainerTypeDto>> CreateContainerType(
        [FromBody] CreateContainerTypeDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateContainerTypeCommand
        {
            Name = request.Name,
            Volume = request.Volume,
            UnitId = request.UnitId,
            ProductTypeIds = request.ProductTypeIds
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerTypeDto>>(
            ct => Ok(ContainerTypeDto.FromDomainModel(ct)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<ContainerTypeDto>> UpdateContainerType(
        [FromBody] UpdateContainerTypeDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateContainerTypeCommand
        {
            Id = request.Id,
            Name = request.Name,
            Volume = request.Volume,
            UnitId = request.UnitId,
            ProductTypeIds = request.ProductTypeIds
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerTypeDto>>(
            ct => Ok(ContainerTypeDto.FromDomainModel(ct)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ContainerTypeDto>> DeleteContainerType(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteContainerTypeCommand
        {
            Id = id
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerTypeDto>>(
            ct => Ok(ContainerTypeDto.FromDomainModel(ct)),
            e => e.ToObjectResult());
    }
}
