using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;

namespace PlatformRestore.Services
{
    public interface IAccessTokenService
    {

        public Task<string> GetToken();

        public Task<InteractiveBrowserCredential> GetCredentials();

        public void InvalidateToken();
    }
}
