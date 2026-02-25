namespace Api.Dtos;

public record GoogleAuthDto(
    string IdToken);

public record AuthResponseDto
{
    public required string Token { get; init; }
}