namespace Application.Common.Exceptions;

public abstract class ContainerException(int containerId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int ContainerId { get; } = containerId;
}

public class ContainerAlreadyExistException(int containerId) 
    : ContainerException(containerId, "Container already exist exception");

public class ContainerNotFoundException(int containerId) 
    : ContainerException(containerId, $"Container not found under id {containerId}");

public class UnhandledContainerException(int containerId, Exception? innerException = null) 
    : ContainerException(containerId, "Unhandled container exception", innerException);