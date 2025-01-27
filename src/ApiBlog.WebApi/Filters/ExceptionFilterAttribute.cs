using ApiBlog.Application.Exceptions;
using ApiBlog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBlog.WebApi.Filters;

public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _handlers;

    public ExceptionFilterAttribute()
    {
        _handlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(PostNotFoundException), HandleNotFoundException },
            { typeof(PostInvalidOperationException), HandleInvalidOperationException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_handlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (PostNotFoundException)context.Exception;
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleInvalidOperationException(ExceptionContext context)
    {
        var exception = (PostInvalidOperationException)context.Exception;
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "The specified operation was invalid.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error ocurred while processing your request."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
