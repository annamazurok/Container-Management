using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Domain;
using Domain.Entities;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Units.Commands;

public class UpdateUnitCommand : IRequest<Either<BaseException, Domain.Entities.Unit>>
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required UnitType UnitType { get; init; }
}

public class UpdateUnitCommandHandler(
    IRepository<Domain.Entities.Unit> unitRepository,
    IUnitQueries unitQueries,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateUnitCommand, Either<BaseException, Domain.Entities.Unit>>
{
    public async Task<Either<BaseException, Domain.Entities.Unit>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitQueries.GetByIdAsync(request.Id, cancellationToken);

        return await unit.MatchAsync(
            u => CheckDuplicates(u.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, u, cancellationToken)),
            () => new UnitNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, Domain.Entities.Unit>> UpdateEntity(
        UpdateUnitCommand request,
        Domain.Entities.Unit unit,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User not authenticated");

            unit.UpdateDetails(
                request.Title,
                request.UnitType,
                userId); 

            return await unitRepository.UpdateAsync(unit, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledUnitException(unit.Id, ex);
        }
    }

    private async Task<Either<BaseException, Unit>> CheckDuplicates(
        int currentUnitId,
        string title,
        CancellationToken cancellationToken)
    {
        var unit = await unitQueries.GetByTitleAsync(title, cancellationToken);

        return unit.Match<Either<BaseException, Unit>>(
            u => u.Id.Equals(currentUnitId) ? Unit.Default : new UnitAlreadyExistException(u.Id),
            () => Unit.Default);
    }
}