using Fcg.Game.Domain.Entities;

namespace Fcg.Game.Application.Repositories
{
	public interface IPurchasedGameRepository
	{
		ValueTask InsertMany(List<PurchasedGameModel> purchasedGameModel);
		ValueTask<List<PurchasedGameModel>> SelectByUserId(Guid guid);
	}
}