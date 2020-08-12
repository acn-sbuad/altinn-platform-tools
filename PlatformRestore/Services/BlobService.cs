using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore.Services
{
    /// <inheritdoc/>
    public class BlobService : IBlobService
    {
        private readonly IBlobContainerClientProvider _clientProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobService"/> class.
        /// </summary>
        /// <param name="clientProvider">The blob container client provider.</param>
        public BlobService(IBlobContainerClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        private async Task ListBlobs()
        {
            BlobContainerClient container = await _clientProvider.GetBlobClient("digdir", Program.Environment);

            // looping through the blobs in the container doesn't seem to work.
            await foreach (Azure.Storage.Blobs.Models.BlobItem item in container.GetBlobsAsync())
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
