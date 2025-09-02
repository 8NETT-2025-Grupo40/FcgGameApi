using Elastic.Clients.Elasticsearch;
using Fcg.Game.Application.Services.Ports;
using Microsoft.Extensions.Logging;

namespace Fcg.Game.Application.Services
{
	public class ElasticService : IElasticService
	{
		private const string INDEX_NAME = "games";
		private readonly ElasticsearchClient _client;
		private readonly ILogger<ElasticService> _logger;

		public ElasticService(ILogger<ElasticService> logger)
		{
			var uri = new Uri("http://localhost:9200");
			var settings = new ElasticsearchClientSettings(uri);
			_client = new ElasticsearchClient(settings);
			_logger = logger;
		}

		public async ValueTask CreateGame()
		{
			await CreateIndex();
			//TODO: Continue here, create new docs on index and to return, validate if index exists.
		}

		private async ValueTask CreateIndex()
		{
			var response = await _client.Indices.CreateAsync(INDEX_NAME);

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
