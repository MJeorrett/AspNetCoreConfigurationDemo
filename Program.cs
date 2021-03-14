using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

                    AddConfigFromAzureKeyVault(configBuilder);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void AddConfigFromAzureKeyVault(IConfigurationBuilder configBuilder)
        {
            var builtConfig = configBuilder.Build();

            var keyVaultName = builtConfig["KeyVaultName"];

            if (keyVaultName is null) return;

            var cred = new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = "069831ec-b8ce-47d2-9463-ef559572c01e" });

            var secretClient = new SecretClient(
                new Uri($"https://{keyVaultName}.vault.azure.net/"),
                cred);

            configBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
    }
}
