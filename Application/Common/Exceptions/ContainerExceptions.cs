namespace Application.Common.Exceptions;

public class ContainerAlreadyExistException(int containerId) 
    : BaseException(containerId, "Container already exist exception");

public class ContainerNotFoundException(int containerId) 
    : BaseException(containerId, $"Container not found under id {containerId}");

public class UnhandledContainerException(int containerId, Exception? innerException = null) 
    : BaseException(containerId, "Unhandled container exception", innerException);