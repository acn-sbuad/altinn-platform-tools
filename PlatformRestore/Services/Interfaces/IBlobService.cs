using System.Threading.Tasks;

namespace PlatformRestore.Services
{
    /// <summary>
    /// The service that handles all interaction with Azure Blob Storage.
    /// </summary>
    public interface IBlobService
    {
        /// <summary>
        /// Lists all blobs.
        /// </summary>
        public Task ListBlobs();
    }
}
