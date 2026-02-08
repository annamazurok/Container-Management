using Api.Dtos;
using Domain;
using LanguageExt;

namespace Api.Services.Abstract;

public interface IUnitControllerService : IControllerService<UnitDto>
{
    Task<Option<UnitDto>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<IReadOnlyList<UnitDto>> GetByUnitTypeAsync(UnitType unitType, CancellationToken cancellationToken);
    
}