using System;
using Azure.Core;
using Azure.Identity;


namespace PlatformRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please log in to use the tool!");
            var clientId = "6840fb87-7b9d-4775-b073-5fdf577c4820";

            var credentials = new InteractiveBrowserCredential(clientId);

            AccessToken token = credentials.GetToken(new TokenRequestContext());

            Console.WriteLine("Your token will expire: {0}", token.ExpiresOn.ToLocalTime() );

            while (true)
            {

            }

        }
    }
}
