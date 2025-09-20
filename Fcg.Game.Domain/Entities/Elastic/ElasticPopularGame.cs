using Fcg.Game.Domain.Exceptions;

namespace Fcg.Game.Domain.Entities.Elastic
{
	public class ElasticPopularGame
	{
		public ElasticPopularGame(Guid gameId, long purchaseCount)
		{
			GameId = gameId;
			PurchaseCount = purchaseCount;
		}

		public ElasticPopularGame(string gameIdStr, long purchaseCount)
		{
			if (string.IsNullOrWhiteSpace(gameIdStr) || !Guid.TryParse(gameIdStr, out var gameId))
			{
				throw new DomainException("Game is invalid");
			}

			GameId = gameId;
			PurchaseCount = purchaseCount;
		}

		public Guid GameId { get; set; }
		public long PurchaseCount { get; set; }
	}
}
