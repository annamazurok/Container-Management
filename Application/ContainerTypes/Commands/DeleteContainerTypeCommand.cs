using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using LanguageExt;
using MediatR;

namespace Application.ContainerTypes.Commands;

public class DeleteContainerTypeCommand : IRequest<Either<BaseException, ContainerType>>
{
    public int Id { get; init; }
}

public class DeleteContainerTypeCommandHandler(
    IRepository<ContainerType> containerTypeRepository,
    IContainerTypeQueries containerTypeQueries,
    IContainerQueries containerQueries,
    IContainerTypeProductTypeRepository containerTypeProductTypeRepository,
    IContainerTypeProductTypeQuery containerTypeProductTypeQuery) : IRequestHandler<DeleteContainerTypeCommand, Either<BaseException, ContainerType>>
{
    public async Task<Either<BaseException, ContainerType>> Handle(DeleteContainerTypeCommand request, CancellationToken cancellationToken)
    {
        var containerType = await containerTypeQueries.GetByIdAsync(request.Id, cancellationToken);

        return await containerType.MatchAsync(
            ct => CheckDependencies(ct.Id, cancellationToken)
                .BindAsync(_ => DeleteEntity(ct, cancellationToken)),
            () => new ContainerTypeNotFoundException(request.Id));
    }

    private async Task<Either<BaseException, ContainerType>> DeleteEntity(
        ContainerType containerType,
        CancellationToken cancellationToken)
    {
        try
        {
            var relations = await containerTypeProductTypeQuery.GetByContainerTypeAsync(containerType.Id, cancellationToken);
            
            if (relations.Any())
                await containerTypeProductTypeRepository.DeleteRangeAsync(relations, cancellationToken);

            await containerTypeRepository.DeleteAsync(containerType, cancellationToken);

            return containerType;
        }
        catch (Exception ex)
        {
            return new UnhandledContainerTypeException(containerType.Id, ex);
        }
    }

    private async Task<Either<BaseException, LanguageExt.Unit>> CheckDependencies(
        int containerTypeId,
        CancellationToken cancellationToken)
    {
        var containers = await containerQueries.GetByContainerTypeAsync(containerTypeId, cancellationToken);

        return containers.Any()
            ? new ContainerTypeHasContainersException(containerTypeId)
            : LanguageExt.Unit.Default;
    }
}