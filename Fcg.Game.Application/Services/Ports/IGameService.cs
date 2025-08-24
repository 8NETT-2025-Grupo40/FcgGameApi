using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Domain.Entities;

namespace Fcg.Game.Application.Services.Ports;

public interface IGameService
{
	ValueTask<OperationResult<string>> CreateGame(CreateGameRequest createGameRequest);
	ValueTask<OperationResult<IEnumerable<GameModel>>> ListAvailableGames();
}
