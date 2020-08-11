using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using PlatformRestore.Configuration;

namespace PlatformRestore.Services
{
    /// <inheritdoc/>
    public class AccessTokenService : IAccessTokenService
    {
        private InteractiveBrowserCredential _credential;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenService"/> class.
        /// </summary>
        /// <param name="settings">The general settings.</param>
        public AccessTokenService(IOptions<GeneralSettings> settings)
        {
            GeneralSettings generalSettings = settings.Value;
            _credential = new InteractiveBrowserCredential(generalSettings.TenantId, generalSettings.ClientId);
        }

        /// <inheritdoc/>
        public void InvalidateCredentials()
        {
            _credential = null;
        }

        /// <inheritdoc/>
        public async Task<string> GetToken(TokenRequestContext requestContext)
        {
            AccessToken token = await _credential.GetTokenAsync(requestContext);

            return token.Token;
        }

        /// <inheritdoc/>
        public InteractiveBrowserCredential GetCredential()
        {
            return _credential;
        }
    }
}