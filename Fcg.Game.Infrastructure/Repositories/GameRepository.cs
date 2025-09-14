using Fcg.Game.Application.Repositories;
using Fcg.Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Game.Infrastructure.Repositories;

public class GameRepository(DatabaseGameContext databaseGameContext) : IGameRepository
{
	public async ValueTask Insert(GameModel gameModel)
	{
		databaseGameContext.Games.Add(gameModel);
		await databaseGameContext.SaveChangesAsync();
	}

	public async ValueTask<IEnumerable<GameModel>> SelectAll() => 
		await databaseGameContext.Games.AsNoTracking().ToListAsync();

	public async ValueTask<GameModel?> SelectById(Guid gameId) =>
		await databaseGameContext.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id.Equals(gameId));
}
