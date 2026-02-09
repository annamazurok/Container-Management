namespace Application.Common.Exceptions;

public class UserAlreadyExistsException(int userId)
    : BaseException(userId, $"User already exists exception");

public class UserNotFoundException(int userId)
    : BaseException(userId, $"User not found under id {userId}");

public class UnhandledUserException(int userId, Exception? innerException = null)
    : BaseException(userId, "Unhandled user exception", innerException);