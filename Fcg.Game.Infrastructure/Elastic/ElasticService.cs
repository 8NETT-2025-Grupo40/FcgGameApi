using Elastic.Clients.Elasticsearch;
using Fcg.Game.Application.Elastic;
using Microsoft.Extensions.Logging;
#if RELEASE
using Elastic.Transport;
#endif

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticService<T> : IElasticService<T>
	{
		private readonly string _indexName;
		private readonly ILogger _logger;

		protected readonly ElasticsearchClient Client;

		public ElasticService(IElasticSettings elasticSettings, ILoggerFactory loggerFactory)
		{
			_indexName = typeof(T).Name.ToLower();


#if DEBUG
			const string elasticAddress = "http://localhost:9200";
			var settings = new ElasticsearchClientSettings(new Uri(elasticAddress));
#else
			var nodePool = new CloudNodePool(elasticSettings.CloudId, new ApiKey(elasticSettings.Key));

			var settings = new ElasticsearchClientSettings(nodePool);
#endif

			Client = new ElasticsearchClient(settings);

			_logger = loggerFactory.CreateLogger(nameof(T));
		}

		public async ValueTask CreateDocumentAsync(T document)
		{
			await CreateIndex();

			var indexResponse = await Client.IndexAsync(document, x => x.Index(_indexName));

			if (indexResponse.IsValidResponse is false)
			{
				if (indexResponse.TryGetOriginalException(out var ex) && ex is not null)
				{
					_logger.LogError("Could not create document\n{ExceptionMessage}", ex.Message);
					return;
				}

				_logger.LogError("Could not create document");

				return;
			}

			_logger.LogInformation("Document created");
		}

		public async ValueTask CreateManyDocumentsAsync(IReadOnlyCollection<T> documents)
		{
			await CreateIndex();

			var response = await Client.BulkAsync(b => b.Index(_indexName).IndexMany(documents));

			if (response.IsValidResponse is false)
			{
				if (response.TryGetOriginalException(out var ex) && ex is not null)
				{
					_logger.LogError("Could not create document\n{ExceptionMessage}", ex.Message);
					return;
				}

				_logger.LogError("Could not create document");

				return;
			}

			_logger.LogInformation("Document created");
		}

		public async ValueTask<IReadOnlyCollection<T>> GetAllAsync() =>
			(await Client.SearchAsync<T>(s => s.Indices(_indexName))).Documents;

		private async ValueTask CreateIndex()
		{
			var response = await Client.Indices.CreateAsync(_indexName);

			if (response.ApiCallDetails.HttpStatusCode is 400)
			{
				return;
			}

			if (response.ApiCallDetails.HttpStatusCode is not 200)
			{
				var errorMessage =
					string.IsNullOrWhiteSpace(response.ApiCallDetails?.OriginalException?.Message) ?
						$"Could not create index\nError code:{response?.ApiCallDetails?.HttpStatusCode}" :
							response.ApiCallDetails.OriginalException.Message;

				_logger.LogError(errorMessage);

				return;
			}

			_logger.LogInformation("Index created successfully");
		}
	}
}
