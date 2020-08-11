using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlatformRestore.Configuration;
using PlatformRestore.Services;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore
{
    /// <summary>
    /// Program.
    /// </summary>
    public class Program
    {
        private static readonly string _prompt = "Altinn Platform Restore > ";
        private static IConfigurationRoot _configuration;

        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            ConfigureSetupLogging();
            _configuration = BuildConfiguration();

            IServiceCollection services = GetAndRegisterServices();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ILoggerFactory>();

            var app = serviceProvider.GetService<ApplicationManager>();

            string args;
            while (true)
            {
                Console.Write(_prompt);
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
            /* // Setup logging for the web host creation
             var logFactory = LoggerFactory.Create(builder =>
             {
                 builder
                     .AddFilter("Altinn.Platform.Restore.Program", LogLevel.Debug)
                     .AddConsole();
             });

             _logger = logFactory.CreateLogger<Program>();*/
        }

        private static IServiceCollection GetAndRegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging();
            services.AddTransient<ApplicationManager>();
            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddSingleton<ICosmosService, CosmosService>();
            services.AddSingleton<IBlobContainerClientProvider, BlobContainerClientProvider>();
            services.AddSingleton<IDocumentClientProvider, DocumentClientProvider>();

            services.Configure<GeneralSettings>(_configuration.GetSection("GeneralSettings"));
            services.Configure<AzureStorageConfiguration>(_configuration.GetSection("AzureStorageConfiguration"));
            services.Configure<AzureCosmosConfiguration>(_configuration.GetSection("AzureCosmosConfiguration"));

            return services;
        }
    }
}
