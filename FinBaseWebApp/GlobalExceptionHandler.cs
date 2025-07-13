using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

public class GlobalExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(HttpActionExecutedContext context)
    {
        var exception = context.Exception;

        Console.WriteLine($"Exception Message: {exception.Message}");
        Console.WriteLine($"Exception StackTrace: {exception.StackTrace}");

        if (exception.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {exception.InnerException.Message}");
        }

        var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new
        {
            Message = "Error processing request",
            ExceptionType = exception.GetType().Name,
            ExceptionMessage = exception.Message,   
            InnerExceptionMessage = exception.InnerException?.Message,      
            StackTrace = exception.StackTrace       
        });

        context.Response = response; 
    }
}
