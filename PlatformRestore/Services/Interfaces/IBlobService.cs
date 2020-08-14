using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Storage.Blobs.Models;
using PlatformRestore.Enums;

namespace PlatformRestore.Services
{
    /// <summary>
    /// The service that handles interaction with Azure Blob Storage.
    /// </summary>
    public interface IBlobService
    {      
        /// <summary>
        /// Lists blobs related to the instance in the given state(s).
        /// </summary>
        public Task<List<BlobItem>> ListBlobs(string org, string app, string instanceGuid, ElementState state);

        /// <summary>
        /// Lists all available snapshots (versions) of the blob.
        /// </summary>
        public Task<List<string>> ListBlobVersions(string org, string app, string instanceGuid, string dataGuid);
    }
}
