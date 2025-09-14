using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Extensions;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticService : IElasticService
	{
		private readonly string _indexName;
		private readonly ElasticsearchClient _client;
		private readonly ILogger<ElasticService> _logger;

		public ElasticService(IElasticSettings elasticSettings, ILogger<ElasticService> logger)
		{
			_logger = logger;

			_indexName = typeof(ElasticGameModel).Name.ToLower();
			var settings = new ElasticsearchClientSettings(new Uri(elasticSettings.Address));
			_client = new ElasticsearchClient(settings);
		}

		public async ValueTask CreateDocumentAsync(ElasticGameModel document)
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

			_logger.LogInformation("Document created");
		}

		public async ValueTask<IReadOnlyCollection<ElasticGameModel>> GetAllAsync() =>
			(await _client.SearchAsync<ElasticGameModel>(s => s.Indices(_indexName))).Documents;

		public async ValueTask<IReadOnlyCollection<ElasticGameModel>> GetSuggestionsAsync(Genre genre, HashSet<string> ownedGames, int pageSize)
		{
			var queries = new List<Query>();
			
			string gameIdPropertyName = ClassExtensions.GetPropertyName((ElasticGameModel c) => c.GameId);
			string genrePropertyName = ClassExtensions.GetPropertyName((ElasticGameModel c) => c.Genre);

			foreach (var item in ownedGames)
			{
				queries.Add(new MatchQuery() { Field = gameIdPropertyName, Query = item });
			}

			var searchResponse = await _client.SearchAsync<ElasticGameModel>(s => s
				.Query(q => q
					.Bool(b => b
						.Should(new List<Query>
						{
							new MatchQuery() { Field = genrePropertyName,Query = (int)genre }
						})
						.MustNot(queries)
					)
				)
			);

			return searchResponse.Documents;
		}

		private async ValueTask CreateIndex()
		{
			var response = await _client.Indices.CreateAsync(_indexName);

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
