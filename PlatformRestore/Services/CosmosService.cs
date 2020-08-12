using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Altinn.Platform.Storage.Interface.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace PlatformRestore.Services.Interfaces
{
    /// <inheritdoc/>
    public class CosmosService : ICosmosService
    {
        private readonly IDocumentClientProvider _clientProvider;
        private readonly Uri _instanceCollectionUri;
        private readonly Uri _dataCollectionUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosService"/> class.
        /// </summary>
        /// <param name="clientProvider">The document client provider.</param>
        public CosmosService(IDocumentClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
            _instanceCollectionUri = UriFactory.CreateDocumentCollectionUri("Storage", "instances");
            _dataCollectionUri = UriFactory.CreateDocumentCollectionUri("Storage", "dataElements");
        }

        /// <inheritdoc/>
        public async Task<DataElement> GetDataElement(string dataGuid, string instanceGuid = "")
        {
            DataElement dataElement = new DataElement();

            Uri uri = UriFactory.CreateDocumentUri("Storage", "dataElements", dataGuid);
            DocumentClient client = await _clientProvider.GetDocumentClient(Program.Environment);

            if (!string.IsNullOrEmpty(instanceGuid))
            {
                dataElement = await client.ReadDocumentAsync<DataElement>(uri, new RequestOptions { PartitionKey = new PartitionKey(instanceGuid) });
            }
            else
            {
                IDocumentQuery<DataElement> query = client
                    .CreateDocumentQuery<DataElement>(_dataCollectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(i => i.Id == dataGuid)
                    .AsDocumentQuery();

                FeedResponse<DataElement> result = await query.ExecuteNextAsync<DataElement>();
                dataElement = result.First();
            }

            return dataElement;
        }

        /// <inheritdoc/>
        public async Task<List<string>> ListDataElements(string instanceGuid)
        {
            List<string> dataGuids = new List<string>();
            string continuationToken = null;

            DocumentClient client = await _clientProvider.GetDocumentClient(Program.Environment);

            do
            {
                var feed = await client.ReadDocumentFeedAsync(
                    _dataCollectionUri,
                    new FeedOptions
                    {
                        PartitionKey = new PartitionKey(instanceGuid),
                        RequestContinuation = continuationToken
                    });

                continuationToken = feed.ResponseContinuation;

                foreach (Document document in feed)
                {
                    dataGuids.Add(document.Id);
                }
            }
            while (continuationToken != null);

            return dataGuids;
        }
    }
}
