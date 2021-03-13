using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AspNetCoreConfigurationDemo.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ConfigController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("providers")]
        public ActionResult GetProviders()
        {
            string result = "";
            var configRoot = (IConfigurationRoot)_config;

            foreach (var provider in (configRoot.Providers.ToList()))
            {
                result += provider.ToString() + "\n";
            }

            return Ok(result);
        }
    }
}
