using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Altinn.Platform.Storage.Interface.Models;

using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace PlatformRestore.Services.Interfaces
{
    /// <inheritdoc/>
    public class CosmosService : ICosmosService
    {
        private readonly IDocumentClientProvider _clientProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosService"/> class.
        /// </summary>
        /// <param name="clientProvider">The document client provider.</param>
        public CosmosService(IDocumentClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        /// <inheritdoc/>
        public async Task ListDeletedInstances()
        {
            DocumentClient client = await _clientProvider.GetDocumentClient("tt02");
            IDocumentQuery<Instance> query = client.CreateDocumentQuery<Instance>(UriFactory.CreateDocumentCollectionUri("Storage", "instances")).AsDocumentQuery();
            FeedResponse<Instance> result = await query.ExecuteNextAsync<Instance>();
            List<Instance> instances = result.ToList<Instance>();

            Console.WriteLine($"{instances.Count} instances were retrieved from tt02");
        }
    }
}
