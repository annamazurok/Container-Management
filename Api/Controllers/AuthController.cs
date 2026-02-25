using Api.Dtos;
using Api.Modules.Errors;
using Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] GoogleAuthDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterWithGoogleCommand
        {
            IdToken = request.IdToken
        };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match<ActionResult<AuthResponseDto>>(
            token => Ok(new AuthResponseDto { Token = token }),
            e => e.ToObjectResult());
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] GoogleAuthDto request,
        CancellationToken cancellationToken)
    {
        var command = new LoginWithGoogleCommand
        {
            IdToken = request.IdToken
        };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match<ActionResult<AuthResponseDto>>(
            token => Ok(new AuthResponseDto { Token = token }),
            e => e.ToObjectResult());
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = User.FindFirst("name")?.Value;
        var surname = User.FindFirst("surname")?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId = int.Parse(userId ?? "0"),
            email,
            name,
            surname,
            role
        });
    }
}
