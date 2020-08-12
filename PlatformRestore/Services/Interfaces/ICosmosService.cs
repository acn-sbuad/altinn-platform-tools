using System.Collections.Generic;
using System.Threading.Tasks;

using Altinn.Platform.Storage.Interface.Models;

namespace PlatformRestore.Services.Interfaces
{
    /// <summary>
    /// The service that handles all interaction with Azure Cosmos DB.
    /// </summary>
    public interface ICosmosService
    {
        /// <summary>
        /// List the data guid for all active blobs for the given instance.
        /// </summary>
        public Task<List<string>> ListDataElements(string instanceGuid);

        /// <summary>
        /// Gets metadata for the given data element.
        /// </summary>
        public Task<DataElement> GetDataElement(string dataGuid, string instanceGuid);
    }
}
