using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Queries;
using Domain;
using LanguageExt;

namespace Api.Services.Implementation;

public class ContainerControllerService(IContainerQueries containerQueries) : IContainerControllerService
{
    public async Task<IReadOnlyList<ContainerDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetAllAsync(cancellationToken);
        
        return result.Select(ContainerDto.FromDomainModel).ToList();
    }

    public async Task<Option<ContainerDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByIdAsync(id, cancellationToken);

        return result.Match(
            c => ContainerDto.FromDomainModel(c),
            () => Option<ContainerDto>.None
        );
    }

    public async Task<Option<ContainerDto>> GetByNameAsync(string containerName, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByNameAsync(containerName, cancellationToken);

        return result.Match(
            c => ContainerDto.FromDomainModel(c),
            () => Option<ContainerDto>.None
        );
    }

    public async Task<Option<ContainerDto>> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByNameAsync(code, cancellationToken);

        return result.Match(
            c => ContainerDto.FromDomainModel(c),
            () => Option<ContainerDto>.None
        );
    }

    public async Task<IReadOnlyList<ContainerDto>> GetByTypeAsync(int typeId, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByContainerTypeAsync(typeId, cancellationToken);
        
        return result.Select(ContainerDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<ContainerDto>> GetByProductAsync(int productId, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByProductAsync(productId, cancellationToken);
        
        return result.Select(ContainerDto.FromDomainModel).ToList();
    }

    public async Task<IReadOnlyList<ContainerDto>> GetByStatusAsync(Status status, CancellationToken cancellationToken)
    {
        var result = await containerQueries.GetByStatusAsync(status, cancellationToken);
        
        return result.Select(ContainerDto.FromDomainModel).ToList();
    }
}