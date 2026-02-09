using Api.Dtos;
using Api.Services.Abstract;
using Domain;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController(
    IHistoryControllerService historyService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            h => Ok(h),
            () => NotFound());
    }

    [HttpGet("container/{containerId:int}")]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetByContainer(
        int containerId,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetByContainerAsync(containerId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("product/{productId:int}")]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetByProduct(
        int productId,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetByProductAsync(productId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("action-type/{actionType}")]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetByActionType(
        Status actionType,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetByActionTypeAsync(actionType, cancellationToken);
        return Ok(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetByUser(
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetByUserAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("recent/{count:int}")]
    public async Task<ActionResult<IReadOnlyList<HistoryDto>>> GetRecent(
        int count,
        CancellationToken cancellationToken)
    {
        var result = await historyService.GetRecentHistoryAsync(count, cancellationToken);
        return Ok(result);
    }
}
