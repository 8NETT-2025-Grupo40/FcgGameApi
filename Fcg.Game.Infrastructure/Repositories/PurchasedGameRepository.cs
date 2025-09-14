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

	public async ValueTask<List<PurchasedGameModel>> SelectByUserId(Guid guid)
	{
		var a = await databaseGameContext.PurchasedGames.Where(p => p.UserIdentifier.Equals(guid)).ToListAsync();
		var c = await databaseGameContext.PurchasedGames.Where(p => p.UserIdentifier == guid).ToListAsync();
		var b = await databaseGameContext.PurchasedGames.Select(p => p.UserIdentifier.Equals(guid)).ToListAsync();
		return a;
	}
}
