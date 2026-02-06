namespace Application.Common.Exceptions;

public class ProductAlreadyExistsException(int productId)
    : BaseException (productId, $"Product already exists exception");

public class ProductNotFoundException(int productId)
    : BaseException(productId, $"Product not found under id {productId}");

public class UnhandledProductException(int productId, Exception? innerException = null)
    : BaseException(productId, "Unhandled product exception", innerException);