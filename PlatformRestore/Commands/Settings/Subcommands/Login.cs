﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using McMaster.Extensions.CommandLineUtils;
using PlatformRestore.Services;

namespace PlatformRestore.Commands.Settings.Subcommands
{
    /// <summary>
    /// Logout command handler. Subcommand of Settings.
    /// </summary>
    [Command(
     Name = "Login",
     Description = "Login using Azure AD. All credential will be saved locally.",
     OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
     UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class Login : IBaseCmd
    {
        private readonly IAccessTokenService _accessTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        /// <summary>
        /// Prompts user for credentials.
        /// </summary>
        protected override async Task OnExecuteAsync(CommandLineApplication app)
        {
            try
            {
                _accessTokenService.InvalidateCredentials();
                await _accessTokenService.GetToken(new TokenRequestContext(new[] { "https://management.azure.com/user_impersonation" }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
