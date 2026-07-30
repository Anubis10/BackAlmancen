using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackAlmancen.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Tomar como ejemplo el proyecto MVC C#
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            // MAndar un Bad Request si el modelo no es válido
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);

                return;
            }

            await next();
        }
    }
}
