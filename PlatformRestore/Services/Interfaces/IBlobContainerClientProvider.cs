using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace PlatformRestore.Services.Interfaces
{
    /// <summary>
    /// Service that provides and administers BlobContainerClients for Azure Blob Storage.
    /// </summary>
    public interface IBlobContainerClientProvider
    {
        /// <summary>
        /// Retrieves a blob client for the given context.
        /// </summary>
        /// <param name="org">The organisation that owns the blob container.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>The blob container client.</returns>
        Task<BlobContainerClient> GetBlobClient(string org, string environment);

        /// <summary>
        /// Invalidates the blob container client if it exists.
        /// </summary>
        /// <param name="org">The organisation that owns the blob container.</param>
        /// <param name="environment">The environment.</param>
        void InvalidateBlobClient(string org, string environment);

        /// <summary>
        /// Deletes all cached blob clientes.
        /// </summary>
        void RemoveBlobClients();
    }
}
