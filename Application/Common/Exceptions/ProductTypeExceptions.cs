namespace Application.Common.Exceptions;


public class ProductTypeAlreadyExistsException(int productTypeId)
    : BaseException(productTypeId, $"Product type already exists exception");

public class ProductTypeNotFoundException(int productTypeId)
    : BaseException(productTypeId, $"Product type not found under id {productTypeId}");

public class UnhandledProductTypeException(int productTypeId, Exception? innerException = null)
    : BaseException(productTypeId, "Unhandled product type exception", innerException);