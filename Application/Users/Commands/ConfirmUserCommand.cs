using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands;

public class ConfirmUserCommand : IRequest<Either<BaseException, User>>
{
    public required int UserId { get; init; }
}

public class ConfirmUserCommandHandler(
    IRepository<User> userRepository,
    IUserQueries userQueries,
    IEmailService emailService,
    ICurrentUserService currentUserService)
    : IRequestHandler<ConfirmUserCommand, Either<BaseException, User>>
{
    public async Task<Either<BaseException, User>> Handle(
        ConfirmUserCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
                            ?? throw new UnauthorizedException("User not authenticated");

        var user = await userQueries.GetByIdAsync(request.UserId, cancellationToken);

        return user.Match<Either<BaseException, User>>(
            u =>
            {
                u.ConfirmEmail(currentUserId);
                emailService.SendUserConfirmedEmailAsync(currentUserService.Email);
                return userRepository.UpdateAsync(u, cancellationToken).Result;
            },
            () => new UserNotFoundException(request.UserId.ToString()));
    }
}