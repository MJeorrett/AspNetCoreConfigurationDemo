using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreConfigurationDemo.Controllers
{
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IConfigurationRoot _configRoot;

        public ConfigController(IConfiguration config)
        {
            _config = config;
            _configRoot = (IConfigurationRoot)config;
        }

        [HttpGet("api/config-providers")]
        public ActionResult GetProviders()
        {
            string result = "";

            foreach (var provider in (_configRoot.Providers.ToList()))
            {
                result += provider.ToString() + "\n";
            }

            return Ok(result);
        }

        [HttpGet("api/config")]
        public ActionResult GetAll()
        {
            var result = "";

            foreach (var configKvp in _config.AsEnumerable())
            {
                result += $"{configKvp.Key} => '{configKvp.Value}'\n";
            }

            return Ok(result);
        }

        [HttpGet("api/config-providers/{index}")]
        public ActionResult GetProvider(int index)
        {
            if (index >= _configRoot.Providers.Count())
            {
                return BadRequest(new
                {
                    message = "Index out of range.",
                });
            }

            var provider = _configRoot.Providers.ElementAt(index);
            var providerConfig = new ConfigurationRoot(new List<IConfigurationProvider> { provider })
                .AsEnumerable();

            var result =
                $"Name\n" +
                $"====\n" +
                $"{provider}\n" +
                $"\n" +
                $"Values\n" +
                $"======\n";

            foreach (var configKvp in providerConfig.AsEnumerable())
            {
                result += $"{configKvp.Key} => '{configKvp.Value}'\n";
            }

            return Ok(result);
        }
    }
}
