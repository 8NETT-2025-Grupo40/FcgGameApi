using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services.Ports;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.ValueObjects;

namespace Fcg.Game.Application.Services;

//TODO: Create unit tests for service.
public class GameService(
	IGameRepository gameRepository,
	IElasticService elasticService) : IGameService
{
	public async ValueTask<OperationResult<string>> CreateGame(CreateGameRequest createGameRequest)
	{
		try
		{
			var title = new Title(createGameRequest.Title);
			var description = new Description(createGameRequest.Description);
			var price = new Price(createGameRequest.Price);

			var game = new GameModel(title, description, createGameRequest.Genre, createGameRequest.ReleaseDate, price)
			{
				DateCreated = DateTime.Now,
			};

			await gameRepository.Insert(game);

			await elasticService.CreateGame();

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