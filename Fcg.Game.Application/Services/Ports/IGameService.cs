using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Domain.Entities;

namespace Fcg.Game.Application.Services.Ports;

public interface IGameService
{
	ValueTask<OperationResult> AddGameToLibrary(GrantGameRequest grantGameRequest);
	ValueTask<OperationResult<string>> CreateGame(CreateGameRequest createGameRequest);
	ValueTask<OperationResult<List<ElasticGameModel>>> GetSuggestions(Guid guid);
	ValueTask<OperationResult<IEnumerable<GameModel>>> ListAvailableGames();
	ValueTask<OperationResult<List<GameModel>>> RetrieveGamesById(Guid guid);
}
