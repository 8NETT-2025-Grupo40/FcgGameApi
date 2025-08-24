using Fcg.Game.Domain.Entities;

namespace Fcg.Game.Domain.ValueObjects;

public class PlayerProfileGame
{
	protected PlayerProfileGame() { }

	public PlayerProfileGame(Guid playerProfileId, Guid gameId)
	{
		PlayerProfileId = playerProfileId;
		GameId = gameId;
		AcquiredAt = DateTime.UtcNow;
	}

	public Guid PlayerProfileId { get; private set; }
	public PlayerProfile PlayerProfile { get; private set; } = null!;

	public Guid GameId { get; private set; }
	public GameModel Game { get; private set; } = null!;

	public DateTime AcquiredAt { get; private set; }
}