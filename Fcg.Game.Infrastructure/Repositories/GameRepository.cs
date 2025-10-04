using Fcg.Game.Application.Repositories;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Game.Infrastructure.Repositories;

public class GameRepository(DatabaseGameContext databaseGameContext) : IGameRepository
{
	public async ValueTask<string> Insert(GameModel gameModel)
	{
		var addedGame = databaseGameContext.Games.Add(gameModel);

		await databaseGameContext.SaveChangesAsync();

		return addedGame.Entity.Id.ToString();
	}

	public async ValueTask<IEnumerable<GameModel>> SelectAll() => 
		await databaseGameContext.Games.AsNoTracking().ToListAsync();

	public async ValueTask<GameModel?> SelectById(Guid gameId) =>
		await databaseGameContext.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id.Equals(gameId));

	public async ValueTask<Genre> SelectGenreById(Guid gameId) =>
		await databaseGameContext.Games.Where(g => g.Id.Equals(gameId)).Select(g => g.Genre).FirstOrDefaultAsync();

	public async ValueTask<Title?> SelectNameById(Guid gameId) =>
		await databaseGameContext.Games.AsNoTracking().Where(g => g.Id.Equals(gameId)).Select(g => g.Title).FirstOrDefaultAsync();
}