using Fcg.Game.Domain.Entities.Elastic;

namespace Fcg.Game.Application.Elastic
{
	public interface IElasticPurchasedGameService : IElasticService<ElasticPurchasedGameModel>
	{
		ValueTask<IReadOnlyCollection<ElasticPopularGame>> SearchPopularGames();
	}
}
