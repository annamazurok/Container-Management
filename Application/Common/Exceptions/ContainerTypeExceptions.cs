namespace Application.Common.Exceptions;

public class ContainerTypeAlreadyExistException(int containerTypeId) 
    : BaseException(containerTypeId, "ContainerType already exist exception");

public class ContainerTypeNotFoundException(int containerTypeId) 
    : BaseException(containerTypeId, $"ContainerType not found under id {containerTypeId}");

public class UnhandledContainerTypeException(int containerTypeId, Exception? innerException = null) 
    : BaseException(containerTypeId, "Unhandled containerType exception", innerException);
    
public class ContainerTypeHasContainersException(int containerTypeId)
    : BaseException(containerTypeId, $"ContainerType with id {containerTypeId} has related containers and cannot be deleted");