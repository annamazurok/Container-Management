using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.ProductTypes.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("product-types")] 
[Authorize]
[ApiController]
public class ProductTypeController(
    ISender sender,
    IProductTypeControllerService productTypeControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductTypeDto>>> GetProductTypes(
        CancellationToken cancellationToken)
    {
        var result = await productTypeControllerService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductTypeDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await productTypeControllerService.GetByIdAsync(id, cancellationToken);

        return result.Match<ActionResult<ProductTypeDto>>(
            pt => Ok(pt),
            () => NotFound());
    }

    [HttpGet("title/{title}")]
    public async Task<ActionResult<ProductTypeDto>> GetByTitle(
        [FromRoute] string title,
        CancellationToken cancellationToken)
    {
        var result = await productTypeControllerService.GetByTitleAsync(title, cancellationToken);

        return result.Match<ActionResult<ProductTypeDto>>(
            pt => Ok(pt),
            () => NotFound());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductTypeDto>> CreateProductType(
        [FromBody] CreateProductTypeDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductTypeCommand
        {
            Title = request.Title
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductTypeDto>>(
            pt => Ok(ProductTypeDto.FromDomainModel(pt)),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<ProductTypeDto>> UpdateProductType(
        [FromBody] ProductTypeDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductTypeCommand
        {
            Id = request.Id,
            Title = request.Title
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductTypeDto>>(
            pt => Ok(ProductTypeDto.FromDomainModel(pt)),
            e => e.ToObjectResult());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductTypeDto>> DeleteProductType(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductTypeCommand
        {
            Id = id
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductTypeDto>>(
            pt => Ok(ProductTypeDto.FromDomainModel(pt)),
            e => e.ToObjectResult());
    }
}