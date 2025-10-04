using Elastic.Clients.Elasticsearch.Aggregations;
using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Extensions;
using Fcg.Game.Domain.Entities.Elastic;
using Microsoft.Extensions.Logging;

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticPurchasedGameService : ElasticService<ElasticPurchasedGameModel>, IElasticPurchasedGameService
	{
		public ElasticPurchasedGameService(IElasticSettings elasticSettings, ILoggerFactory loggerFactory) : base(elasticSettings, loggerFactory)
		{
		}

		public async ValueTask<IReadOnlyCollection<ElasticPopularGame>> SearchPopularGames()
		{
			const string keyword = "keyword";
			string gameIdPropertyName = ClassExtensions.GetPropertyName((ElasticPurchasedGameModel c) => c.GameIdentifier);

			var response = await Client.SearchAsync<object>(s => s
				.Size(0)
				.Aggregations(a => a
					.Add("most_popular_games", v => v
						.Terms(t => t
							.Field($"{gameIdPropertyName}.{keyword}")
							.Size(10))
						)
					)
				);

			var aggregations = response.Aggregations;

			if (aggregations?.Values is null ||
				aggregations.Values.FirstOrDefault() is not StringTermsAggregate value)
			{
				return [];
			}

			var elasticPopularGames = new List<ElasticPopularGame>();

			foreach (var bucket in value.Buckets)
			{
				if (bucket.Key.Value is null ||
					bucket.Key.Value is not string gameId ||
					string.IsNullOrWhiteSpace(gameId))
				{
					continue;
				}

				var purchaseCount = bucket.DocCount;

				elasticPopularGames.Add(new ElasticPopularGame(gameId, purchaseCount));
			}

			return elasticPopularGames;
		}
	}
}
