using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Interview.App_Start.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            int statusCode = context.Exception switch
            {
                ArgumentNullException => 400,  
                UnauthorizedAccessException => 401,  
                KeyNotFoundException => 404,  
                _ => 500 
            };

            context.Result = new JsonResult(new
            {
                Error = context.Exception.Message, 
                ExceptionType = context.Exception.GetType().Name 
            })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }

}
