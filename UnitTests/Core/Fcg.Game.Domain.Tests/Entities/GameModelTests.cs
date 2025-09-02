using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Domain.Exceptions;
using Fcg.Game.Domain.ValueObjects;

namespace UnitTests.Core.Fcg.Game.Domain.Tests.Entities;

public class GameModelTests
{
	[Fact]
	public void CreateGame_ShouldInitializePropertiesCorrectly()
	{
		// Arrange
		string title = "Joguinho maneiro";
		string description = "Um jogo massa";
		Genre genre = Genre.Action;
		DateTime releaseDate = new DateTime(2024, 10, 15);
		Price price = new Price(59.99m);

		// Act
		var game = new GameModel(title, description, genre, releaseDate, price);

		// Assert
		Assert.Equal(title, game.Title);
		Assert.Equal(description, game.Description);
		Assert.Equal(genre, game.Genre);
		Assert.Equal(releaseDate, game.ReleaseDate);
		Assert.Equal(price, game.Price);
	}

	[Theory]
	[InlineData("")]
	[InlineData("  ")]
	[InlineData(null)]
	public void CreateGame_ButInvalidTitle_ShouldThrow(string invalidTitle)
	{
		var ex = Assert.Throws<DomainException>(() =>
			new GameModel(invalidTitle, "desc", Genre.Action, DateTime.Today, new Price(10))
		);
		Assert.Equal("The title must not be empty", ex.Message);
	}

	[Theory]
	[InlineData("")]
	[InlineData("  ")]
	[InlineData(null)]
	public void CreateGame_ButInvalidDescription_ShouldThrow(string invalidDescription)
	{
		var ex = Assert.Throws<DomainException>(() =>
			new GameModel("Valid title", invalidDescription, Genre.Action, DateTime.Today, new Price(10))
		);
		Assert.Equal("The description must not be empty", ex.Message);
	}

	[Fact]
	public void CreateGame_ButUnknownGenre_ShouldThrow()
	{
		const string validTitle = "sample valid title";
		const string validDescription = "sample valid description";
		var ex = Assert.Throws<DomainException>(() =>
			new GameModel(validTitle, validDescription, Genre.Unknown, DateTime.Today, new Price(10))
		);
		Assert.Equal("Genre must be specified.", ex.Message);
	}

	[Fact]
	public void CreateGame_ButNegativePrice_ShouldThrow()
	{
		const string validTitle = "sample valid title";
		const string validDescription = "sample valid description";
		var ex = Assert.Throws<DomainException>(() =>
			new GameModel(validTitle, validDescription, Genre.Action, DateTime.Today, new Price(-5))
		);
		Assert.Equal("Price cannot be negative.", ex.Message);
	}
}
