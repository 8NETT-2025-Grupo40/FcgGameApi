using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;

namespace Fcg.Game.Application.Elastic
{
	public interface IElasticGameService : IElasticService<ElasticGameModel>
	{
		ValueTask<IReadOnlyCollection<ElasticGameModel>> GetSuggestionsAsync(Genre genre, HashSet<string> ownedGames, int pageSize);
	}
}
