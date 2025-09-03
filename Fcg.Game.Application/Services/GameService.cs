using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services.Ports;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.ValueObjects;

namespace Fcg.Game.Application.Services;

public class GameService(
	IGameRepository gameRepository,
	IElasticService<ElasticGameModel> elasticService) : IGameService
{
	public async ValueTask<OperationResult<string>> CreateGame(CreateGameRequest createGameRequest)
	{
		try
		{
			var title = new Title(createGameRequest.Title);
			var description = new Description(createGameRequest.Description);
			var price = new Price(createGameRequest.Price);

			var gameModel = new GameModel(title, description, createGameRequest.Genre, createGameRequest.ReleaseDate, price)
			{
				DateCreated = DateTime.Now,
			};

			await gameRepository.Insert(gameModel);

			var elasticGameModel =
				new ElasticGameModel(gameModel.Title, gameModel.Description, (int)gameModel.Genre, gameModel.ReleaseDate, gameModel.Price);

			await elasticService.CreateDocumentAsync(elasticGameModel);

			return OperationResult<string>.CreateSucessfulResponse("Game created successfully");
		}
		catch (Exception exception)
		{
			return OperationResult<string>.CreateErrorResponse(exception.Message);
		}
	}

	public async ValueTask<OperationResult<IEnumerable<GameModel>>> ListAvailableGames()
	{
		try
		{
			var games = await gameRepository.SelectAll();

			if (games.Any() is false)
			{
				return OperationResult<IEnumerable<GameModel>>.CreateErrorResponse("No games available");
			}

			return OperationResult<IEnumerable<GameModel>>.CreateSucessfulResponse(games);
		}
		catch (Exception exception)
		{
			return OperationResult<IEnumerable<GameModel>>.CreateErrorResponse(exception.Message);
		}
	}
}