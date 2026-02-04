namespace Application.Common.Exceptions;

public abstract class ProductTypeException(int typeId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int TypeId { get; } = typeId;
}

public class ProductTypeAlreadyExistsException(int typeId)
    : ProductTypeException(typeId, $"Product type with id {typeId} already exists");

public class ProductTypeNotFoundException(int typeId)
    : ProductTypeException(typeId, $"Product type with id {typeId} was not found");

public class UnhandledProductTypeException(int typeId, Exception? innerException = null)
    : ProductTypeException(typeId, "Unhandled product type exception", innerException);