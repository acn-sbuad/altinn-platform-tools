using System.Threading.Tasks;

namespace PlatformRestore.Services.Interfaces
{
    /// <summary>
    /// The service that handles all interaction with Azure Cosmos DB.
    /// </summary>
    public interface ICosmosService
    {
        /// <summary>
        /// Lists deleted instances 
        /// </summary>
        public Task ListDeletedInstances();
    }
}
