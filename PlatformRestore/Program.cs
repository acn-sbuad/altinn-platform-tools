using System;
using System.IO;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using PlatformRestore.Services;

namespace PlatformRestore
{
    class Program
    {
        private static string prompt = "Altinn Platform Restore > ";
        private static ILogger<Program> _logger;

        static void Main()
        {
            ConfigureSetupLogging();
            IServiceCollection services = GetAndRegisterServices();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IConfigurationRoot configuration = BuildConfiguration();

            var app = serviceProvider.GetService<ApplicationManager>();
            // app.SetEnvironment(configuration, serviceProvider);

            /*

                        Console.WriteLine("Your token will expire: {0}", token.ExpiresOn.ToLocalTime());
            */

            string args;
            while (true)
            {
                Console.Write(prompt);
                args = Console.ReadLine();
                try
                {
                    app.Execute(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Error : {ex.Message}");
                }
            }
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        /// <summary>
        /// Configure logging for setting up application. Temporary
        /// </summary>
        public static void ConfigureSetupLogging()
        {
            // Setup logging for the web host creation
            var logFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Altinn.Platform.Restore.Program", LogLevel.Debug)
                    .AddConsole();
            });

            _logger = logFactory.CreateLogger<Program>();
        }

        private static IServiceCollection GetAndRegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.AddConsole();
            }).AddTransient<ApplicationManager>();

            services.AddTransient<ApplicationManager>();
            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddSingleton<IBlobService, BlobService>();
            return services;
        }
    }


}
