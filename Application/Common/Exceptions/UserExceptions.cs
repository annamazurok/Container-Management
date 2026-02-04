namespace Application.Common.Exceptions;

public abstract class UserException(int userId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int UserId { get; } = userId;
}

public class UserAlreadyExistsException(int userId)
    : UserException(userId, $"User with id {userId} already exists");

public class UserNotFoundException(int userId)
    : UserException(userId, $"User with id {userId} was not found");

public class UnhandledUserException(int userId, Exception? innerException = null)
    : UserException(userId, "Unhandled user exception", innerException);