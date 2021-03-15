namespace Account.API.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public void Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Response.StatusCode = context.Error switch
            {
                ValidationException _ => 400,
                _ => 500,
            };
        }
    }
}
