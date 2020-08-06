using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Identity.Client;

namespace PlatformRestore.Services
{
    public class BlobService : IBlobService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly string _blobEndpoint = "https://{0}altinn{1}storage01.blob.core.windows.net/";
        private readonly string _storageContainer = "{0}-{1}-appsdata-blob-db";
        private readonly string _clientId = "6840fb87-7b9d-4775-b073-5fdf577c4820";

        public BlobService(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        public async Task<bool> ListBlobs()
        {
            //   await _accessTokenService.GetToken();

            BlobContainerClient container = await CreateOrgBlobClient("ttd", "tt02");

            // looping through the blobs in the container doesn't seem to work.
            await foreach (Azure.Storage.Blobs.Models.BlobItem item in container.GetBlobsAsync())
            {
                Console.WriteLine(item.Name);
            }

            return true;
        }

        private async Task<BlobContainerClient> CreateOrgBlobClient(string org, string environment)
        {
            string uri = string.Format(_blobEndpoint, org, environment);
            string containerName = string.Format(_storageContainer, org, environment);
            BlobServiceClient commonBlobClient = new BlobServiceClient(new Uri(uri), await _accessTokenService.GetCredentials());

            var c = commonBlobClient.GetBlobContainersAsync();

            // this works
            await foreach (var container in c)
            {
                Console.WriteLine(container.Name);
            }

            BlobContainerClient blobContainerClient = commonBlobClient.GetBlobContainerClient(containerName);



            return blobContainerClient;
        }
    }
}
