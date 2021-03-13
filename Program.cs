using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreConfigurationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                {
                    configBuilder.Sources.Clear();
                    configBuilder.AddConfiguration(hostingContext.Configuration);

                    var env = hostingContext.HostingEnvironment;

                    configBuilder.AddJsonFile("appsettings.json", true);
                    configBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                    configBuilder.AddUserSecrets<Startup>();
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
