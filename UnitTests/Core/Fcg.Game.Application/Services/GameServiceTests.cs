using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Enums;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace UnitTests.Core.Fcg.Game.Application.Services
{
	public class GameServiceTests
	{
		private GameService _gameService;

		private IGameRepository _gameRepository;
		private IElasticService<ElasticGameModel> _elasticService;

		public GameServiceTests()
		{
			_gameRepository = Substitute.For<IGameRepository>();
			_elasticService = Substitute.For<IElasticService<ElasticGameModel>>();

			_gameService = new GameService(_gameRepository, _elasticService);
		}

		[Fact]
		public async Task OnListAvailableGames_WhenThereAreNoGames_ShouldReturnErrorWithExpectedMessage()
		{
			// Arrange
			const string expectedErrorMessage = "No games available";

			IEnumerable<GameModel> emptyEnumerable = [];

			_gameRepository.SelectAll().Returns(emptyEnumerable);

			// Act
			var result = await _gameService.ListAvailableGames();


			// Assert
			Assert.False(result.IsSuccessful);
			Assert.Equal(expectedErrorMessage, result.Message);
			Assert.Null(result.Value);
			await _gameRepository.Received(1).SelectAll();
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
			await _gameRepository.Received(1).SelectAll();
		}

		[Fact]
		public async Task OnCreateGame_WhenRequestIsCorrect_ShouldSaveGameAndReturnSuccess()
		{
			// Arrange
			const string expectedResultMessage = "Success";
			const string expectedValue = "Game created successfully";
			const string sampleTitle = "sampleTitle";
			const string sampleDescription = "sampleDescription";
			const decimal samplePrice = 1;
			const Genre sampleGenre = Genre.Action;
			var sampleReleaseDate = DateTime.Now;

			var createGameRequest = new CreateGameRequest()
			{
				Description = sampleDescription,
				Genre = sampleGenre,
				ReleaseDate = sampleReleaseDate,
				Price = samplePrice,
				Title = sampleTitle
			};

			// Act
			var result = await _gameService.CreateGame(createGameRequest);


			// Assert

			Assert.True(result.IsSuccessful);
			Assert.Equal(expectedResultMessage, result.Message);
			Assert.Equal(expectedValue, result.Value);
			await _gameRepository.Received(1).Insert(Arg.Is<GameModel>(x => 
				x.Title.Value.Equals(sampleTitle, StringComparison.OrdinalIgnoreCase) && 
				x.Description.Value.Equals(sampleDescription, StringComparison.OrdinalIgnoreCase) && 
				x.Genre == sampleGenre && 
				x.ReleaseDate == sampleReleaseDate &&
				x.Price == samplePrice
			));

			await _elasticService.Received(1).CreateDocumentAsync(Arg.Any<ElasticGameModel>());
		}

		[Fact]
		public async Task OnCreateGame_WhenExcpetionIsThrownInsideMethod_ShouldReturnErrorWithCorrectMessage()
		{
			// Arrange
			const string expectedResultMessage = "sampleExceptionMessage";
			const string expectedValue = null;
			const string sampleTitle = "sampleTitle";
			const string sampleDescription = "sampleDescription";
			const decimal samplePrice = 1;
			const Genre sampleGenre = Genre.Action;
			var sampleReleaseDate = DateTime.Now;

			var createGameRequest = new CreateGameRequest()
			{
				Description = sampleDescription,
				Genre = sampleGenre,
				ReleaseDate = sampleReleaseDate,
				Price = samplePrice,
				Title = sampleTitle
			};

			var exceptionToThrow = new Exception(expectedResultMessage);
			_gameRepository.Insert(Arg.Any<GameModel>()).Throws(exceptionToThrow);

			// Act
			var result = await _gameService.CreateGame(createGameRequest);


			// Assert
			Assert.False(result.IsSuccessful);
			Assert.Equal(expectedResultMessage, result.Message);
			Assert.Equal(expectedValue, result.Value);
		}
	}
}
