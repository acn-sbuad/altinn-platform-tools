using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Identity.Client;

namespace PlatformRestore.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private string _accessTokenString;
        private InteractiveBrowserCredential _credential;
        private string _clientId = "6840fb87-7b9d-4775-b073-5fdf577c4820";
        private string _tenantId = "cd0026d8-283b-4a55-9bfa-d0ef4a8ba21c";
        private string[] _scopes = new string[] { "User.Read" };


        public void InvalidateToken()
        {
            _accessTokenString = string.Empty;
            _credential = null;
        }

        public async Task<string> GetToken()
        {
            var clientId = "6840fb87-7b9d-4775-b073-5fdf577c4820";

            // Using token acuisition. Must figure out how to DI into clas
            /*
            var tenant = "cd0026d8-283b-4a55-9bfa-d0ef4a8ba21c";
            return await _tokenAcquisition.GetAccessTokenForUserAsync(scopes, tenant);*/


            // using azure identity
            if (string.IsNullOrEmpty(_accessTokenString))
            {
                var credentials = new InteractiveBrowserCredential(clientId);
                var token = await credentials.GetTokenAsync(new TokenRequestContext());
                _accessTokenString = token.Token;
            }

            return _accessTokenString;
        }

        public async Task<InteractiveBrowserCredential> GetCredentials()
        {
            if (_credential == null)
            {
                _credential = new InteractiveBrowserCredential(_clientId);
              //  await _credential.GetTokenAsync(new TokenRequestContext());
            }

            return _credential;
        }
    }
}

