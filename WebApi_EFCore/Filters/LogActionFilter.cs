using Microsoft.AspNetCore.Mvc.Filters;

namespace POSSystem2.Filters
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(
                $"Action started: {context.ActionDescriptor.DisplayName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(
                $"Action finished: {context.ActionDescriptor.DisplayName}");
        }
    }
}