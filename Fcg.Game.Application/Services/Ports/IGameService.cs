using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Entities.Elastic;

namespace Fcg.Game.Application.Services.Ports;

public interface IGameService
{
	ValueTask<OperationResult> AddGameToLibrary(GrantGameRequest grantGameRequest);
	ValueTask<OperationResult<string>> CreateGame(CreateGameRequest createGameRequest);
	ValueTask<OperationResult<List<ElasticGameModel>>> GetSuggestions(Guid guid);
	ValueTask<OperationResult<IEnumerable<GameModel>>> ListAvailableGames();
	ValueTask<OperationResult<IReadOnlyCollection<PopularGame>>> Popular();
	ValueTask<OperationResult<List<GameModel>>> RetrieveGamesById(Guid guid);
}
