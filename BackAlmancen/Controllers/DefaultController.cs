using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace BackAlmancen.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"Petición realizada a version 1.0");
            return "Running v1";
        }

    }
}
