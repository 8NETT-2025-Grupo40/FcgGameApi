﻿using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Domain.ValueObjects;

namespace UnitTests.Core.Fcg.Game.Domain.Tests.ValueObjects;

public class PlayerProfileTests
{
	[Fact]
	public void CreatePlayerProfile_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		Guid userId = Guid.NewGuid();
		Nickname nickname = new("Gamer123");

		// Act
		PlayerProfile profile = new(userId, nickname);

		// Assert
		Assert.Equal(userId, profile.UserId);
		Assert.Equal(nickname, profile.Nickname);
		Assert.NotNull(profile.Games);
		Assert.Empty(profile.Games);
	}

	[Fact]
	public void AddGame_ShouldAddOnlyOnce()
	{
		// Arrange
		var profile = new PlayerProfile(Guid.NewGuid(), new Nickname("Jogador1"));
		var game = new GameModel("Zelda Breath of The Wild", "jogo eletrônico no estilo hack and slash", Genre.Action, DateTime.Now.AddYears(-1), 59.99m);

		// Act
		profile.AddGame(game);
		// A segunda chamada não deve duplicar
		profile.AddGame(game);

		// Assert
		Assert.Single(profile.Games);
		var added = profile.Games.First();
		Assert.Equal(game.Id, added.GameId);
		Assert.Equal(profile.Id, added.PlayerProfileId);
	}
}
