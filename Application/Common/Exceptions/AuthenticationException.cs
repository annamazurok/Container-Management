using Application.Common.Exceptions;

public class AuthenticationException : BaseException
{
    public AuthenticationException(string message)
        : base(0, message)  
    {
    }
}

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message)
        : base(0, message)  
    {
    }
}