using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Containers.Commands;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("containers")]
[ApiController]
public class ContainerController(
    ISender sender,
    IContainerControllerService containerControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContainerDto>>> GetContainers(
        CancellationToken cancellationToken)
    {
         var result = await containerControllerService.GetAllAsync(cancellationToken);
         return Ok(result);
    }

    [HttpGet("{containerId:int}")]
    public async Task<ActionResult<ContainerDto>> GetById(
        [FromRoute] int containerId,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByIdAsync(containerId, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(c),
            () => NotFound());
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<ContainerDto>> GetByName(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByNameAsync(name, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(c),
            () => NotFound());
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ContainerDto>> GetByCode(
        [FromRoute] string code,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByCodeAsync(code, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(c),
            () => NotFound());
    }

    [HttpGet("type/{typeId:int}")]
    public async Task<ActionResult<IReadOnlyList<ContainerDto>>> GetByType(
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByTypeAsync(typeId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("product/{productId:int}")]
    public async Task<ActionResult<IReadOnlyList<ContainerDto>>> GetByProduct(
        [FromRoute] int productId,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByProductAsync(productId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<ContainerDto>>> GetByStatus(
        [FromRoute] Status status,
        CancellationToken cancellationToken)
    {
        var result = await containerControllerService.GetByStatusAsync(status, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ContainerDto>> CreateContainer(
        [FromBody] CreateContainerDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateContainerCommand
        {
            Name = request.Name,
            Code = request.Code,
            TypeId = request.TypeId,
            ProductId = request.ProductId ?? 0,
            Quantity = request.Quantity ?? 0,
            UnitId = request.UnitId ?? 0,
            Notes = request.Notes
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(ContainerDto.FromDomainModel(c)),
            e => e.ToObjectResult());
    }
    
    [HttpPut]
    public async Task<ActionResult<ContainerDto>> UpdateContainer(
        [FromBody] UpdateContainerDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateContainerCommand
        {
            Id = request.Id,
            Name = request.Name,
            TypeId = request.TypeId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitId = request.UnitId,
            Notes = request.Notes
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(ContainerDto.FromDomainModel(c)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{containerId:int}")]
    public async Task<ActionResult<ContainerDto>> DeleteContainer(
        [FromRoute] int containerId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteContainerCommand
        {
            Id = containerId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ContainerDto>>(
            c => Ok(ContainerDto.FromDomainModel(c)),
            e => e.ToObjectResult());
    }
}
