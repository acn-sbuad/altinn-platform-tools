using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Altinn.Platform.Storage.Interface.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using PlatformRestore.Commands.Storage.Data;
using PlatformRestore.Enums;

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
        public async Task<DataElement> GetDataElementBackup(string instanceGuid, string dataGuid)
        {
            BlobContainerClient container = await _clientProvider.GetBlobClient("backup", Program.Environment);
            BlockBlobClient client = container.GetBlockBlobClient($"dataElements/{instanceGuid}/{dataGuid}");

            try
            {
                using var stream = new MemoryStream();
                await client.DownloadToAsync(stream);
                string metadata = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                DataElement backup = JsonConvert.DeserializeObject<DataElement>(metadata, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return backup;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
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
            List<string> deletedBlobs = new List<string>();
            List<string> versions = new List<string>();
            int counter = 1;

            BlobContainerClient container = await _clientProvider.GetBlobClient(org, Program.Environment);
            BlockBlobClient client = container.GetBlockBlobClient($"{org}/{app}/{instanceGuid}/data/{dataGuid}");

            await foreach (BlobItem i in container.GetBlobsAsync(BlobTraits.None, BlobStates.Deleted, prefix: client.Name))
            {
                if (i.Deleted)
                {
                    Console.WriteLine($"Deleted blob name: {i.Name}");
                    deletedBlobs.Add(i.Name);
                }
            }

            await client.UndeleteAsync();

            await foreach (BlobItem i in container.GetBlobsAsync(BlobTraits.None, BlobStates.All, prefix: client.Name))
            {
                versions.Add($"Version {counter} \t Last modified {i.Properties.LastModified} \t Restore timestamp: {i.Snapshot ?? "N/A"}");
                counter++;
            }

            // original state for snapshots is deleteing, so restoring this state
            await client.DeleteIfExistsAsync(DeleteSnapshotsOption.OnlySnapshots);

            // deleting the blob itself if it was originally deleted
            foreach (string name in deletedBlobs)
            {
                await container.DeleteBlobIfExistsAsync(name, DeleteSnapshotsOption.None);
            }

            return versions;
        }

        /// <inheritdoc/>
        public async Task<bool> UndeleteBlob(string org, string app, string instanceGuid, string dataGuid)
        {
            BlobContainerClient container = await _clientProvider.GetBlobClient(org, Program.Environment);
            BlockBlobClient client = container.GetBlockBlobClient($"{org}/{app}/{instanceGuid}/data/{dataGuid}");
            try
            {
                await client.UndeleteAsync();
                await client.DeleteIfExistsAsync(DeleteSnapshotsOption.OnlySnapshots);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
