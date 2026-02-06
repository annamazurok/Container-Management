namespace Application.Common.Exceptions;

public abstract class HistoryException(int historyId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int HistoryId { get; } = historyId;
}

public class HistoryAlreadyExistException(int historyId) 
    : HistoryException(historyId, "History already exist exception");

public class HistoryNotFoundException(int historyId) 
    : HistoryException(historyId, $"History not found under id {historyId}");

public class UnhandledHistoryException(int historyId, Exception? innerException = null) 
    : HistoryException(historyId, "Unhandled history exception", innerException);