using Domain;
using LanguageExt;
using Unit = Domain.Entities.Unit;

namespace Application.Common.Interfaces.Queries;

public interface IUnitQueries : IBaseQuery<Unit>
{
    Task<Option<Unit>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<IReadOnlyList<Unit>> GetByUnitTypeAsync(UnitType unitType, CancellationToken cancellationToken);
}