using Fcg.Game.Domain.Base;
using Fcg.Game.Domain.Exceptions;

namespace Fcg.Game.Domain.Entities;

public class PurchasedGameModel : EntityBase
{
	protected PurchasedGameModel() { }

	protected PurchasedGameModel(Guid userIdentifier, Guid gameIdentifier)
	{
		UserIdentifier = userIdentifier;

		GameIdentifier = gameIdentifier;
	}

	public static PurchasedGameModel Create(Guid userIdentifier, Guid gameIdentifier)
	{
		if (userIdentifier == Guid.Empty)
		{
			throw new DomainException("User identifier must be especified");
		}

		if (gameIdentifier == Guid.Empty)
		{
			throw new DomainException("Game identifier must be especified");
		}

		return new PurchasedGameModel(userIdentifier, gameIdentifier);
	}

	public static PurchasedGameModel Create(string userIdentifierStr, string gameIdentifierStr)
	{
		if (string.IsNullOrWhiteSpace(userIdentifierStr))
		{
			throw new DomainException("User identifier must be especified");
		}

		if (string.IsNullOrWhiteSpace(gameIdentifierStr))
		{
			throw new DomainException("Game identifier must be especified");
		}

		if (!Guid.TryParse(userIdentifierStr, out var userIdentifier) || userIdentifier == Guid.Empty)
		{
			throw new DomainException("User identifier must be valid");
		}

		if (!Guid.TryParse(gameIdentifierStr, out var gameIdentifier) || gameIdentifier == Guid.Empty)
		{
			throw new DomainException("Game identifier must be valid");
		}

		return new PurchasedGameModel(userIdentifier, gameIdentifier);
	}

	public Guid UserIdentifier { get; set; }
	public Guid GameIdentifier { get; set; }
}
