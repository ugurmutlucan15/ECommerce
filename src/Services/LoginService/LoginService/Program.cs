using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using System;

namespace LoginService
{
    public class Program
    {
        private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/appsettings.json", optional: false)
                    .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
        }

        private static IConfiguration SeriLogConfiguration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/serilog.json", optional: false)
                    .AddJsonFile($"Configurations/serilog.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
        }

        [Obsolete("Obsolete")]
        public static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(i => i.AddConfiguration(configuration))
                    .UseStartup<Startup>()
                    .ConfigureLogging(i => i.ClearProviders())
                    .UseSerilog()
                    .Build()
                ;
        }

        [Obsolete("Obsolete")]
        public static void Main(string[] args)
        {
            var host = BuildWebHost(Configuration, args);

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(SeriLogConfiguration)
                    .CreateLogger()
                ;
            host.Run();
        }
    }
}