using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain;
using LanguageExt;
using MediatR;
using DomainUnit = Domain.Entities.Unit;

namespace Application.Units.Commands;

public class CreateUnitCommand : IRequest<Either<BaseException, DomainUnit>>
{
    public required string Title { get; init; }
    public required UnitType UnitType { get; init; }
}

public class CreateUnitCommandHandler(
    IRepository<DomainUnit> unitRepository,
    IUnitQueries unitQueries,
    ICurrentUserService currentUserService) : IRequestHandler<CreateUnitCommand, Either<BaseException, DomainUnit>>
{
    public async Task<Either<BaseException, DomainUnit>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var existingUnit = await unitQueries.GetByTitleAsync(request.Title, cancellationToken);

        return await existingUnit.MatchAsync(
            u => new UnitAlreadyExistException(u.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BaseException, DomainUnit>> CreateEntity(
        CreateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User not authenticated");

            var unit = await unitRepository.CreateAsync(
                DomainUnit.New(
                    request.Title,
                    request.UnitType,
                    userId), 
            cancellationToken);

            return unit;
        }
        catch (Exception ex)
        {
            return new UnhandledUnitException(0, ex);
        }
    }
}