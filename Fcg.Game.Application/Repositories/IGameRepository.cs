using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;

namespace Fcg.Game.Application.Repositories;

public interface IGameRepository
{
	ValueTask<string> Insert(GameModel gameModel);
	ValueTask<IEnumerable<GameModel>> SelectAll();
	ValueTask<GameModel?> SelectById(Guid gameId);
	ValueTask<Genre> SelectGenreById(Guid gameId);
}
