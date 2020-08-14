using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using PlatformRestore.Enums;
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

        /// <inheritdoc/>
        public async Task<List<BlobItem>> ListBlobs(string org, string app, string instanceGuid, ElementState state = ElementState.Active)
        {
            List<BlobItem> blobs = new List<BlobItem>();
            BlobStates blobState = state.Equals(ElementState.Active) ? BlobStates.None : BlobStates.Deleted;

            BlobContainerClient container = await _clientProvider.GetBlobClient(org, Program.Environment);

            await foreach (BlobItem item in container.GetBlobsAsync(BlobTraits.None, blobState, $"{org}/{app}/{instanceGuid}"))
            {
                Console.WriteLine(item.Name);
                blobs.Add(item);
            }

            if (blobs.Count > 0 && state.Equals(ElementState.Deleted))
            {
                blobs = blobs.Where(b => b.Deleted == true).ToList();
            }

            return blobs;
        }

        /// <inheritdoc/>
        public async Task<List<string>> ListBlobVersions(string org, string app, string instanceGuid, string dataGuid)
        {
            List<string> versions = new List<string>();
            int counter = 1;

            BlobContainerClient container = await _clientProvider.GetBlobClient(org, Program.Environment);
            BlockBlobClient client = container.GetBlockBlobClient($"{org}/{app}/{instanceGuid}/data/{dataGuid}");
            await client.UndeleteAsync();

            await foreach (BlobItem i in container.GetBlobsAsync(BlobTraits.None, BlobStates.All, prefix: client.Name))
            {
                versions.Add($"Version {counter} \t Last modified {i.Properties.LastModified} \t Restore timestamp: {i.Snapshot ?? "N/A"}");
                counter++;
            }

            await client.DeleteIfExistsAsync(DeleteSnapshotsOption.OnlySnapshots);

            return versions;
        }
    }
}
