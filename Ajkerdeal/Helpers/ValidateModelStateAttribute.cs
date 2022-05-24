using Microsoft.AspNetCore.Mvc.Filters;

namespace Ajkerdeal.Helpers
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //context.Result = new BadRequestObjectResult(context.ModelState);
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
