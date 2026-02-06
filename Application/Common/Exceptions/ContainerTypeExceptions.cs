namespace Application.Common.Exceptions;

public abstract class ContainerTypeException(int containerTypeId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int ContainerTypeId { get; } = containerTypeId;
}

public class ContainerTypeAlreadyExistException(int containerTypeId) 
    : ContainerTypeException(containerTypeId, "ContainerType already exist exception");

public class ContainerTypeNotFoundException(int containerTypeId) 
    : ContainerTypeException(containerTypeId, $"ContainerType not found under id {containerTypeId}");

public class UnhandledContainerTypeException(int containerTypeId, Exception? innerException = null) 
    : ContainerTypeException(containerTypeId, "Unhandled containerType exception", innerException);