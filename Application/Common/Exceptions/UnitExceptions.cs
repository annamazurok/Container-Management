namespace Application.Common.Exceptions;

public abstract class UnitException(int unitId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int UnitId { get; } = unitId;
}

public class UnitAlreadyExistException(int unitId) 
    : UnitException(unitId, "Unit already exist exception");

public class UnitNotFoundException(int unitId) 
    : UnitException(unitId, $"Unit not found under id {unitId}");

public class UnhandledUnitException(int unitId, Exception? innerException = null) 
    : UnitException(unitId, "Unhandled unit exception", innerException);