﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using PlatformRestore.Configuration;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore.Services
{
    /// <inheritdoc/>
    public class BlobContainerClientProvider : IBlobContainerClientProvider
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly Dictionary<string, StorageAccountConfig> _accountConfig;
        private readonly Dictionary<string, BlobContainerClient> _clients = new Dictionary<string, BlobContainerClient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobContainerClientProvider"/> class.
        /// </summary>
        /// <param name="accessTokenService">The access token service.</param>
        /// <param name="storageConfig">The storage configuration.</param>
        public BlobContainerClientProvider(IAccessTokenService accessTokenService, IOptions<AzureStorageConfiguration> storageConfig)
        {
            _accountConfig = storageConfig.Value.AccountConfig;
            _accessTokenService = accessTokenService;
        }

        /// <inheritdoc/>
        public async Task<BlobContainerClient> GetBlobClient(string org, string environment)
        {
            string key = $"{org}-{environment}";

            if (_clients.TryGetValue(key, out BlobContainerClient client))
            {
                return client;
            }

            try
            {
                _accountConfig.TryGetValue(key, out StorageAccountConfig config);

                if (string.IsNullOrEmpty(config.AccountKey))
                {
                    StorageManagementClient storageManagementClient = new StorageManagementClient(config.SubscriptionId, _accessTokenService.GetCredential());
                    StorageAccountListKeysResult res = await storageManagementClient.StorageAccounts.ListKeysAsync(config.ResourceGroup, config.AccountName);
                    config.AccountKey = res.Keys.Where(k => k.KeyName == "key1").FirstOrDefault().Value;
                    _accountConfig[key].AccountKey = config.AccountKey;
                }

                string blobEndpoint = $"https://{config.AccountName}.blob.core.windows.net/";

                BlobServiceClient commonBlobClient = new BlobServiceClient(new Uri(blobEndpoint), new StorageSharedKeyCredential(config.AccountName, config.AccountKey));
                client = commonBlobClient.GetBlobContainerClient($"{org}-{environment}-appsdata-blob-db");

                _clients.TryAdd(key, client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return client;
        }

        /// <inheritdoc/>
        public void InvalidateBlobClient(string org, string environment)
        {
            string key = $"{org}-{environment}";
            _clients.Remove(key);
        }

        /// <inheritdoc/>
        public void RemoveBlobClients()
        {
            foreach (var entry in _accountConfig)
            {
                entry.Value.AccountKey = string.Empty;
            }

            _clients.Clear();
        }
    }
}
