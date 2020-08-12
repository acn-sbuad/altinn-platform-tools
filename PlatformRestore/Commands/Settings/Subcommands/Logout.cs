using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using PlatformRestore.Services;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore.Commands.Settings.Subcommands
{
    /// <summary>
    /// Logout command handler. Subcommand of Settings.
    /// </summary>
    [Command(
     Name = "Logout",
     Description = "Logout. All stored Azure credentials and clients will be deleted.",
     OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
     UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class Logout : IBaseCmd
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IBlobContainerClientProvider _blobContainerClientProvider;
        private readonly IDocumentClientProvider _documentClientProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logout"/> class.
        /// </summary>
        public Logout(
            IAccessTokenService accessTokenService,
            IBlobContainerClientProvider blobContainerClientProvider,
            IDocumentClientProvider documentClientProvider)
        {
            _accessTokenService = accessTokenService;
            _blobContainerClientProvider = blobContainerClientProvider;
            _documentClientProvider = documentClientProvider;
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        protected override Task OnExecuteAsync(CommandLineApplication app)
        {
            _accessTokenService.InvalidateCredentials();
            _blobContainerClientProvider.RemoveBlobClients();
            _documentClientProvider.RemoveDobumentClients();

            Console.WriteLine("All credentials and clients successfully removed.");
            return Task.CompletedTask;
        }
    }
}
