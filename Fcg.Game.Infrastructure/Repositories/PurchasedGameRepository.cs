using Fcg.Game.Application.Repositories;
using Fcg.Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Game.Infrastructure.Repositories;

public class PurchasedGameRepository(DatabaseGameContext databaseGameContext) : IPurchasedGameRepository
{
	public async ValueTask InsertMany(List<PurchasedGameModel> purchasedGameModel)
	{
		await databaseGameContext.PurchasedGames.AddRangeAsync(purchasedGameModel);
		await databaseGameContext.SaveChangesAsync();
	}

	public async ValueTask<List<PurchasedGameModel>> SelectByUserId(Guid guid) =>
		await databaseGameContext.PurchasedGames.Where(p => p.UserIdentifier.Equals(guid)).ToListAsync();

	public async ValueTask<List<Guid>> SelectGameIdentifiersByUserId(Guid guid) =>
		await databaseGameContext.PurchasedGames.Where(p => p.UserIdentifier.Equals(guid)).Select(p => p.GameIdentifier).ToListAsync();
}
