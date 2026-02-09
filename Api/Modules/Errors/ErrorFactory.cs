using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ErrorFactory
{
    public static ObjectResult ToObjectResult(this BaseException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                ContainerAlreadyExistException => StatusCodes.Status409Conflict,
                ContainerNotFoundException => StatusCodes.Status404NotFound,
                UnhandledContainerException => StatusCodes.Status500InternalServerError,

                ContainerTypeAlreadyExistException => StatusCodes.Status409Conflict,
                ContainerTypeNotFoundException => StatusCodes.Status404NotFound,
                ContainerTypeHasContainersException => StatusCodes.Status409Conflict,
                
                HistoryAlreadyExistException => StatusCodes.Status409Conflict,
                HistoryNotFoundException => StatusCodes.Status404NotFound,
                
                UnitAlreadyExistException => StatusCodes.Status409Conflict,
                UnitNotFoundException => StatusCodes.Status404NotFound,
                UnitHasContainersException  => StatusCodes.Status409Conflict,
                UnitHasContainerTypesException  => StatusCodes.Status409Conflict,
                UnitHasHistoriesException => StatusCodes.Status409Conflict,

                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
