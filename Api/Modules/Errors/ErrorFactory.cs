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
                UnhandledContainerTypeException => StatusCodes.Status500InternalServerError,

                HistoryAlreadyExistException => StatusCodes.Status409Conflict,
                HistoryNotFoundException => StatusCodes.Status404NotFound,
                UnhandledHistoryException => StatusCodes.Status500InternalServerError,

                UnitAlreadyExistException => StatusCodes.Status409Conflict,
                UnitNotFoundException => StatusCodes.Status404NotFound,
                UnitHasContainersException  => StatusCodes.Status409Conflict,
                UnitHasContainerTypesException  => StatusCodes.Status409Conflict,
                UnitHasHistoriesException => StatusCodes.Status409Conflict,
                UnhandledUnitException => StatusCodes.Status500InternalServerError,

                ProductAlreadyExistsException => StatusCodes.Status409Conflict,
                ProductNotFoundException => StatusCodes.Status404NotFound,
                UnhandledProductException => StatusCodes.Status500InternalServerError,

                ProductTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                ProductTypeNotFoundException => StatusCodes.Status404NotFound,
                ProductTypeHasProductsException => StatusCodes.Status409Conflict,
                UnhandledProductTypeException => StatusCodes.Status500InternalServerError,

                UserAlreadyExistsException => StatusCodes.Status409Conflict,
                UserNotFoundException => StatusCodes.Status404NotFound,
                UnhandledUserException => StatusCodes.Status500InternalServerError,

                RoleNotFoundException => StatusCodes.Status404NotFound,

                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
