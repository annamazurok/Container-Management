namespace Application.Common.Exceptions;

public abstract class ProductException(int productId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int ProductId { get; } = productId;
}

public class ProductAlreadyExistsException(int productId)
    : ProductException(productId, $"Product with id {productId} already exists");

public class ProductNotFoundException(int productId)
    : ProductException(productId, $"Product with id {productId} was not found");

public class UnhandledProductException(int productId, Exception? innerException = null)
    : ProductException(productId, "Unhandled product exception", innerException);