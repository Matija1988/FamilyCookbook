using Microsoft.AspNetCore.Mvc.Filters;

namespace FamilyCookbook.Exceptions.Handler
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid) 
            { 
                var errors = context.ModelState
                    .Where(modelState => modelState.Value.Errors.Any())
                    .ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.Errors.Select(e=> e.ErrorMessage).ToArray()
                    );

                throw new ModelValidationException(errors);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
