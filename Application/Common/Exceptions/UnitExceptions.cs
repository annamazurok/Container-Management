namespace Application.Common.Exceptions;

public class UnitAlreadyExistException(int unitId) 
    : BaseException(unitId, "Unit already exist exception");

public class UnitNotFoundException(int unitId) 
    : BaseException(unitId, $"Unit not found under id {unitId}");

public class UnhandledUnitException(int unitId, Exception? innerException = null) 
    : BaseException(unitId, "Unhandled unit exception", innerException);
    
public class UnitHasContainersException(int unitId)
    : BaseException(unitId, $"Unit with id {unitId} has related containers and cannot be deleted");

public class UnitHasContainerTypesException(int unitId)
    : BaseException(unitId, $"Unit with id {unitId} has related container types and cannot be deleted");

public class UnitHasHistoriesException(int unitId)
    : BaseException(unitId, $"Unit with id {unitId} has related histories and cannot be deleted");


