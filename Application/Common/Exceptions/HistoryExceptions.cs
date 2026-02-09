namespace Application.Common.Exceptions;

public class HistoryAlreadyExistException(int historyId) 
    : BaseException(historyId, "History already exist exception");

public class HistoryNotFoundException(int historyId) 
    : BaseException(historyId, $"History not found under id {historyId}");

public class UnhandledHistoryException(int historyId, Exception? innerException = null) 
    : BaseException(historyId, "Unhandled history exception", innerException);