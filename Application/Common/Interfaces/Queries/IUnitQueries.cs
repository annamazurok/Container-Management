using Domain;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IUnitQueries
{
    Task<IReadOnlyList<Unit>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<Unit>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Option<Unit>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<IReadOnlyList<Unit>> GetByUnitTypeAsync(UnitType unitType, CancellationToken cancellationToken);
}