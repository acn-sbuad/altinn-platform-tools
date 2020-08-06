using Microsoft.Extensions.Logging;
using PlatformRestore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformRestore
{
    public class ApplicationManager
    {
        private readonly ILogger<ApplicationManager> _logger;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IBlobService _blobService;

        public ApplicationManager(ILogger<ApplicationManager> logger, IAccessTokenService accessTokenService, IBlobService blobService)
        {
            _logger = logger;
            _accessTokenService = accessTokenService;
            _blobService = blobService;
        }

        public void Execute(string args)
        {
            string[] input = args.ToLower().Split(' ');

            switch (input[0])
            {
                case "login":
                    _accessTokenService.GetToken();
                    break;
                case "blob":
                    _blobService.ListBlobs();
                    break;
                default:
                    Console.WriteLine($"Unknown argument {args}. Please try again.");
                    break;
            }
        }
    }
}
