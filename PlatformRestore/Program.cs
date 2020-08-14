using System;
using System.IO;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PlatformRestore.Commands.Settings;
using PlatformRestore.Commands.Storage;
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
        /// <summary>
        /// Global environment used in commands.
        /// </summary>
        public static string Environment { get; set; }

        private static readonly string _prompt = "Altinn Platform Restore";     
        private static readonly CommandLineApplication<Settings> _settingsCmd = new CommandLineApplication<Settings>();
        private static readonly CommandLineApplication<Storage> _storageCmd = new CommandLineApplication<Storage>();
        private static IConfigurationRoot _configuration;

        /// <summary>
        /// Main method.
        /// </summary>
        public static async Task Main()
        {
            _configuration = BuildConfiguration();
            IServiceCollection services = GetAndRegisterServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _storageCmd.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);

            _settingsCmd.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(serviceProvider);

            while (true)
            {
                if (string.IsNullOrEmpty(Environment))
                {
                    Console.Write($"{_prompt}> ");
                }
                else
                {
                    Console.Write($"{_prompt} [{Environment}]> ");
                }

                string[] args = Console.ReadLine().Trim().Split(' ');

                switch (args[0].ToLower())
                {
                    case "storage":
                        await _storageCmd.ExecuteAsync(args);
                        break;
                    case "settings":
                        await _settingsCmd.ExecuteAsync(args);
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine($"Unknown argument {string.Join(" ", args)}, Valid commands are storage and settings.");
                        break;
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

        private static IServiceCollection GetAndRegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging();

            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<ICosmosService, CosmosService>();

            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddSingleton<IBlobContainerClientProvider, BlobContainerClientProvider>();
            services.AddSingleton<IDocumentClientProvider, DocumentClientProvider>();

            services.Configure<GeneralSettings>(_configuration.GetSection("GeneralSettings"));
            services.Configure<AzureStorageConfiguration>(_configuration.GetSection("AzureStorageConfiguration"));
            services.Configure<AzureCosmosConfiguration>(_configuration.GetSection("AzureCosmosConfiguration"));

            return services;
        }
    }
}
