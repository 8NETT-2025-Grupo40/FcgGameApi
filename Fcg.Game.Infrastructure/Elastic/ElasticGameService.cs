using Elastic.Clients.Elasticsearch.QueryDsl;
using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Extensions;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Fcg.Game.Infrastructure.Elastic
{
	public class ElasticGameService : ElasticService<ElasticGameModel>, IElasticGameService
	{
		public ElasticGameService(IElasticSettings elasticSettings, ILoggerFactory loggerFactory) : base(elasticSettings, loggerFactory)
		{
		}

		public async ValueTask<IReadOnlyCollection<ElasticGameModel>> GetSuggestionsAsync(Genre genre, HashSet<string> ownedGames, int pageSize)
		{
			var queries = new List<Query>();

			string gameIdPropertyName = ClassExtensions.GetPropertyName((ElasticGameModel c) => c.GameId);
			string genrePropertyName = ClassExtensions.GetPropertyName((ElasticGameModel c) => c.Genre);

			foreach (var item in ownedGames)
			{
				queries.Add(new MatchQuery() { Field = gameIdPropertyName, Query = item });
			}

			var searchResponse = await Client.SearchAsync<ElasticGameModel>(s => s
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
	}
}
