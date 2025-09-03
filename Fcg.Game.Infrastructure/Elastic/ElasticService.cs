using Elastic.Clients.Elasticsearch;
using Fcg.Game.Application.Elastic;
using Microsoft.Extensions.Logging;

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticService<T> : IElasticService<T>
	{
		private readonly string _indexName;
		private readonly ElasticsearchClient _client;
		private readonly ILogger<ElasticService<T>> _logger;

		public ElasticService(IElasticSettings elasticSettings, ILogger<ElasticService<T>> logger)
		{
			_logger = logger;

			_indexName = typeof(T).Name.ToLower();
			var settings = new ElasticsearchClientSettings(new Uri(elasticSettings.Address));
			_client = new ElasticsearchClient(settings);
		}

		public async ValueTask CreateDocumentAsync(T document)
		{
			await CreateIndex();

			var indexResponse = await _client.IndexAsync(document, x => x.Index(_indexName));

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
		}

		public async ValueTask<IReadOnlyCollection<T>> GetAllAsync() =>
			(await _client.SearchAsync<T>(s => s.Indices(_indexName))).Documents;

		public ValueTask<IReadOnlyCollection<T>> GetAsync(int page, int pageSize)
		{
			throw new NotImplementedException();
		}

		private async ValueTask CreateIndex()
		{
			var response = await _client.Indices.CreateAsync(_indexName);

			if (response.ApiCallDetails.HttpStatusCode is 400)
			{
				_logger.LogInformation("Index already exists");
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
