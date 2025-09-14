using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;

namespace Fcg.Game.Application.Elastic
{
	public interface IElasticService
	{
		ValueTask<IReadOnlyCollection<ElasticGameModel>> GetAllAsync();
		ValueTask CreateDocumentAsync(ElasticGameModel document);
		ValueTask<IReadOnlyCollection<ElasticGameModel>> GetSuggestionsAsync(Genre genre, HashSet<string> ownedGames, int pageSize);
	}
}