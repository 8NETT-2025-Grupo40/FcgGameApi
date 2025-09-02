using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using NSubstitute;

namespace UnitTests.Core.Fcg.Game.Application.Services
{
	public class GameServiceTests
	{
		private GameService _gameService;

		private IGameRepository _gameRepository;

		public GameServiceTests()
		{
			_gameRepository = Substitute.For<IGameRepository>();
			_gameService = new GameService(_gameRepository);
		}

		[Fact]
		public async Task OnListAvailableGames_WhenThereAreNoGames_ShouldReturnErrorWithExpectedMessage()
		{
			// Arrange
			const string expectedErrorMessage = "No games available";

			_gameRepository.SelectAll().Returns([]);

			// Act
			var result = await _gameService.ListAvailableGames();


			// Assert
			Assert.False(result.IsSuccessful);
			Assert.Equal(expectedErrorMessage, result.Message);
			Assert.Null(result.Value);
		}

		[Fact]
		public async Task OnListAvailableGames_WhenThereAreNoGames_ShouldReturnListOfGames()
		{
			// Arrange
			const string expectedResultMessage = "Success";
			const string sampleTitle = "sampleTitle";
			const string sampleDescription = "sampleDescription";
			const decimal samplePrice = 1;
			const Genre sampleGenre = Genre.Action;
			var sampleReleaseDate = DateTime.Now;

			var games = new List<GameModel>() { new GameModel(sampleTitle, sampleDescription, sampleGenre, sampleReleaseDate, samplePrice) };
			_gameRepository.SelectAll().Returns(games);

			// Act
			var result = await _gameService.ListAvailableGames();


			// Assert
			var gameResult = result.Value.ToArray()[0];

			Assert.True(result.IsSuccessful);
			Assert.Equal(expectedResultMessage, result.Message);
			Assert.Single(result.Value);
			Assert.IsType<GameModel>(gameResult);
			Assert.Equal(sampleTitle, gameResult.Title.Value);
			Assert.Equal(sampleDescription, gameResult.Description.Value);
			Assert.Equal(samplePrice, gameResult.Price.Value);
			Assert.Equal(sampleGenre, gameResult.Genre);
			Assert.Equal(sampleReleaseDate, gameResult.ReleaseDate);
		}
	}
}
