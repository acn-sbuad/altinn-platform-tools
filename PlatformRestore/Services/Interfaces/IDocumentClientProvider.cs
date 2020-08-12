using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace PlatformRestore.Services.Interfaces
{
    /// <summary>
    /// Service that provides and administers DocumentClients for Azure Cosmos DB.
    /// </summary>
    public interface IDocumentClientProvider
    {
        /// <summary>
        /// Retrieves a document client for the given context.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The document client.</returns>
        Task<DocumentClient> GetDocumentClient(string environment);

        /// <summary>
        /// Invalidates the document client if it exists.
        /// </summary>
        /// <param name="environment">The environment.</param>
        void InvalidateDocumentClient(string environment);

        /// <summary>
        /// Deletes all cached document clientes.
        /// </summary>
        void RemoveDobumentClients();
    }
}
