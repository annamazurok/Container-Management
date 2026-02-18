using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Users.Commands;
public class CreateUserCommand : IRequest<Either<BaseException, User>>
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? FathersName { get; init; }
    public required int RoleId { get; init; }
}

public class CreateUserCommandHandler(
    IRepository<User> userRepository,
    IUserQueries userQueries,
    IRoleQueries roleQueries,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateUserCommand, Either<BaseException, User>>
{
    public async Task<Either<BaseException, User>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var adminId = currentUserService.UserId
                      ?? throw new UnauthorizedException("User not authenticated");
        
        var existingUser = await userQueries.GetByEmailAsync(
            request.Email, cancellationToken);

        return await existingUser.MatchAsync(
            u => new UserAlreadyExistsException(u.Id),
            () => CheckDependencies(request, cancellationToken)
                .BindAsync(_ => CreateEntity(request, cancellationToken)));
    }

    private async Task<Either<BaseException, Unit>> CheckDependencies(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(
            request.RoleId, cancellationToken);

        if (role.IsNone)
            return new RoleNotFoundException(request.RoleId);

        return Unit.Default;
    }

    private async Task<Either<BaseException, User>> CreateEntity(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User not authenticated");

            var user = await userRepository.CreateAsync(
                User.New(
                    request.Email,
                    request.Name,
                    request.Surname,
                    request.FathersName,
                    request.RoleId,
                    userId 
                ),
            cancellationToken);

            return user;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(0, ex);
        }
    }
}