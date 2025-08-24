using Fcg.Game.Domain.Entities;

namespace Fcg.Game.Application.Repositories;

public interface IGameRepository
{
	ValueTask Insert(GameModel gameModel);
	ValueTask<IEnumerable<GameModel>> SelectAll();
}
