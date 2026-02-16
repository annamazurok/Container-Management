using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Services;
using Application.Settings; 
using Google.Apis.Auth;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Options; 

namespace Application.Auth.Commands;

public class LoginWithGoogleCommand : IRequest<Either<BaseException, string>>
{
    public required string IdToken { get; init; }
}

public class LoginWithGoogleCommandHandler(
    IUserQueries userQueries,
    ITokenService tokenService,
    IOptions<GoogleSettings> googleSettings) 
    : IRequestHandler<LoginWithGoogleCommand, Either<BaseException, string>>
{
    private readonly GoogleSettings _googleSettings = googleSettings.Value;

    public async Task<Either<BaseException, string>> Handle(
        LoginWithGoogleCommand request,
        CancellationToken cancellationToken)
    {
        var payload = await VerifyGoogleTokenAsync(request.IdToken);
        if (payload == null)
            return new AuthenticationException("Invalid Google token");

        var user = await userQueries.GetByEmailAsync(payload.Email, cancellationToken);

        if (user.IsNone)
            return new UserNotFoundException($"User with email {payload.Email} not found");

        return user.Match<Either<BaseException, string>>(
            u =>
            {
                if (!u.Confirmed)
                    return new UnauthorizedException("Account not confirmed by administrator");

                var token = tokenService.GenerateToken(u);
                return token;
            },
            () => new UserNotFoundException(payload.Email));
    }

    private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId } 
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch
        {
            return null;
        }
    }
}