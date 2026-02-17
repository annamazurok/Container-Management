using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("products")]
[ApiController]
public class ProductController(
    ISender sender,
    IProductControllerService productControllerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(
        CancellationToken cancellationToken)
    {
        var result = await productControllerService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{productId:int}")]
    public async Task<ActionResult<ProductDto>> GetById(
        [FromRoute] int productId,
        CancellationToken cancellationToken)
    {
        var result = await productControllerService.GetByIdAsync(productId, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            p => Ok(p),
            () => NotFound());
    }

    [HttpGet("type/{typeId:int}")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetByType(
        [FromRoute] int typeId,
        CancellationToken cancellationToken)
    {
        var result = await productControllerService.GetByTypeIdAsync(typeId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("expired")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetExpiredProducts(
        [FromQuery] DateTime? currentDate,
        CancellationToken cancellationToken)
    {
        var date = currentDate ?? DateTime.Now;
        var result = await productControllerService.GetExpiredProductsAsync(date, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            Name = request.Name,
            TypeId = request.TypeId,
            Produced = request.Produced,
            ExpirationDate = request.ExpirationDate,
            Description = request.Description
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            p => Ok(ProductDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> UpdateProduct(
        [FromBody] UpdateProductDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand
        {
            Id = request.Id,
            Name = request.Name,
            TypeId = request.TypeId,
            Produced = request.Produced,
            ExpirationDate = request.ExpirationDate,
            Description = request.Description
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            p => Ok(ProductDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{productId:int}")]
    public async Task<ActionResult<ProductDto>> DeleteProduct(
        [FromRoute] int productId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand
        {
            Id = productId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ProductDto>>(
            p => Ok(ProductDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }
}