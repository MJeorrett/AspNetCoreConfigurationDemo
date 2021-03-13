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

        /// <summary>
        /// Get a list of all configuration providers.
        /// </summary>
        [HttpGet("api/config-providers")]
        public ActionResult GetProviders()
        {
            var result = _configRoot.Providers
                .Select(provider => provider.ToString())
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get a list of all effective configuration.
        /// </summary>
        [HttpGet("api/config")]
        public ActionResult GetAll()
        {
            var result = _config.AsEnumerable()
                .Select(configKvp => $"{configKvp.Key} => '{configKvp.Value}'")
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get a list of all configuration for the provider at the supplied index.
        /// </summary>
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

            var config = providerConfig
                .Select(configKvp => $"{configKvp.Key} => '{configKvp.Value}'")
                .ToList();

            return Ok(new
            {
                name = provider.ToString(),
                config = config,
            });
        }
    }
}
