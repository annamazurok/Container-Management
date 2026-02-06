namespace Application.Common.Exceptions;

public class UnitAlreadyExistException(int unitId) 
    : BaseException(unitId, "Unit already exist exception");

public class UnitNotFoundException(int unitId) 
    : BaseException(unitId, $"Unit not found under id {unitId}");

public class UnhandledUnitException(int unitId, Exception? innerException = null) 
    : BaseException(unitId, "Unhandled unit exception", innerException);