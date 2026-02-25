using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Settings; 
using Domain.Entities;
using Google.Apis.Auth;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Options; 

namespace Application.Auth.Commands;

public class RegisterWithGoogleCommand : IRequest<Either<BaseException, string>>
{
    public required string IdToken { get; init; }
}

public class RegisterWithGoogleCommandHandler(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IUserQueries userQueries,
    IRoleQueries roleQueries,
    ITokenService tokenService,
    IOptions<GoogleSettings> googleSettings) 
    : IRequestHandler<RegisterWithGoogleCommand, Either<BaseException, string>>
{
    private readonly GoogleSettings _googleSettings = googleSettings.Value;

    public async Task<Either<BaseException, string>> Handle(
        RegisterWithGoogleCommand request,
        CancellationToken cancellationToken)
    {
        var payload = await VerifyGoogleTokenAsync(request.IdToken);
        if (payload == null)
            return new AuthenticationException("Invalid Google token");

        var existingUser = await userQueries.GetByEmailAsync(payload.Email, cancellationToken);
        if (existingUser.IsSome)
            return new UserAlreadyExistsException(payload.Email);

        var totalUsers = await userQueries.GetTotalCountAsync(cancellationToken);
        var isFirstUser = totalUsers == 0;

        var roleName = isFirstUser ? "Admin" : "Operator";
        var role = await roleQueries.GetByNameAsync(roleName, cancellationToken);

        if (role.IsNone)
        {
            var newRole = await roleRepository.CreateAsync(
                Role.New(roleName),
                cancellationToken);
            role = newRole;
        }

        var roleId = role.Match(r => r.Id, () => 0);

        var user = User.New(
            email: payload.Email,
            name: payload.GivenName ?? "",
            surname: payload.FamilyName ?? "",
            fathersName: null,
            roleId: roleId,
            createdBy: 0, 
            googleId: payload.Subject
        );

        if (isFirstUser)
        {
            user.ConfirmEmail(0);
        }

        var createdUser = await userRepository.CreateAsync(user, cancellationToken);
        
        if (!createdUser.Confirmed)
        {
            return new UnauthorizedException(
                "Your account is pending approval by administrator. Please wait for confirmation.");
        }

        var token = tokenService.GenerateToken(createdUser);

        return token;
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