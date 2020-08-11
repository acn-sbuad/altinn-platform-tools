using System;
using System.Threading.Tasks;

using Azure.Core;

using PlatformRestore.Services;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore
{
    /// <summary>
    /// Application manager. Logic for the console app
    /// </summary>
    public class ApplicationManager
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IBlobService _blobService;
        private readonly ICosmosService _cosmosService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationManager"/> class.
        /// </summary>  
        public ApplicationManager(IAccessTokenService accessTokenService, IBlobService blobService, ICosmosService cosmosService)
        {
            _accessTokenService = accessTokenService;
            _blobService = blobService;
            _cosmosService = cosmosService;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="args">arguments</param>
        /// <returns></returns>
        public async Task Execute(string args)
        {
            string[] input = args.ToLower().Split(' ');

            switch (input[0])
            {
                case "login":
                    await _accessTokenService.GetToken(new TokenRequestContext(new string[] { }));
                    break;
                case "blob":
                    await _blobService.ListBlobs();
                    break;
                case "cosmos":
                    await _cosmosService.ListDeletedInstances();
                    break;
                default:
                    Console.WriteLine($"Unknown argument {args}. Please try again.");
                    break;
            }
        }
    }
}
