using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Domain;
using LanguageExt;

namespace Api.Services.Implementation;

public class UnitControllerService(
    IUnitQueries unitQueries) : IUnitControllerService
{
    public async Task<IReadOnlyList<UnitDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await unitQueries.GetAllAsync(cancellationToken);
        
        return result.Select(UnitDto.FromDomainModel).ToList();
    }

    public async Task<Option<UnitDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var  result = await unitQueries.GetByIdAsync(id, cancellationToken);

        return result.Match(
            u => UnitDto.FromDomainModel(u),
            () => Option<UnitDto>.None);
    }

    public async Task<Option<UnitDto>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var  result = await unitQueries.GetByTitleAsync(title, cancellationToken);

        return result.Match(
            u => UnitDto.FromDomainModel(u),
            () => Option<UnitDto>.None);
    }

    public async Task<IReadOnlyList<UnitDto>> GetByUnitTypeAsync(UnitType unitType, CancellationToken cancellationToken)
    {
        var result = await unitQueries.GetByUnitTypeAsync(unitType, cancellationToken);
        
        return result.Select(UnitDto.FromDomainModel).ToList();
    }
}