using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Entities;
using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services.Ports;
using Fcg.Game.Domain.Entities;
using Fcg.Game.Domain.Entities.Elastic;
using Fcg.Game.Domain.Enums;
using Fcg.Game.Domain.ValueObjects;

namespace Fcg.Game.Application.Services;

public class GameService(
	IGameRepository gameRepository,
	IPurchasedGameRepository purchasedGameRepository,
	IElasticGameService elasticService,
	IElasticPurchasedGameService elasticPurchasedGameService) : IGameService
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

			var gameId = await gameRepository.Insert(gameModel);

			var elasticGameModel =
				new ElasticGameModel(gameId, gameModel.Title, gameModel.Description, (int)gameModel.Genre, gameModel.ReleaseDate, gameModel.Price);

			await elasticService.CreateDocumentAsync(elasticGameModel);

			return OperationResult<string>.CreateSuccessfulResponse(gameId);
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

			return OperationResult<IEnumerable<GameModel>>.CreateSuccessfulResponse(games);
		}
		catch (Exception exception)
		{
			return OperationResult<IEnumerable<GameModel>>.CreateErrorResponse(exception.Message);
		}
	}

	public async ValueTask<OperationResult> AddGameToLibrary(GrantGameRequest grantGameRequest)
	{
		try
		{
			var userIdStr = grantGameRequest.UserId;
			var purchases = grantGameRequest.PurchasedGames;

			if (!Guid.TryParse(userIdStr, out Guid userId))
			{
				return OperationResult<IEnumerable<PurchasedGameModel>>.CreateErrorResponse("Invalid user identifier");
			}

			var currentPurchases = await purchasedGameRepository.SelectGameIdentifiersByUserId(userId);

			var purchasedGames = new List<PurchasedGameModel>();
			var elasticPurchasedGames = new List<ElasticPurchasedGameModel>();

			foreach (var purchased in grantGameRequest.PurchasedGames)
			{
				if (Guid.TryParse(purchased.GameId, out var gameId) && currentPurchases.Any(c => c.Equals(gameId)))
				{
					continue;
				}

				purchasedGames.Add(PurchasedGameModel.Create(userIdStr, purchased.GameId));
				elasticPurchasedGames.Add(new ElasticPurchasedGameModel { GameIdentifier = purchased.GameId, UserIdentifier = userIdStr });
			}

			if (purchasedGames.Count > 0)
			{
				await purchasedGameRepository.InsertMany(purchasedGames);
			}

			if (elasticPurchasedGames.Count > 0)
			{
				await elasticPurchasedGameService.CreateManyDocumentsAsync(elasticPurchasedGames);

			}

			return OperationResult.CreateSuccessfulResponse();
		}
		catch (Exception exception)
		{
			return OperationResult<IEnumerable<PurchasedGameModel>>.CreateErrorResponse(exception.Message);
		}
	}

	public async ValueTask<OperationResult<List<GameModel>>> RetrieveGamesById(Guid guid)
	{
		try
		{
			var purchasedGames = await purchasedGameRepository.SelectByUserId(guid);

			if (purchasedGames is null || purchasedGames.Count is 0)
			{
				return OperationResult<List<GameModel>>.CreateErrorResponse("No games available");
			}

			var games = new List<GameModel>();

			foreach (var purchasedGame in purchasedGames)
			{
				var dbGame = await gameRepository.SelectById(purchasedGame.GameIdentifier);

				if (dbGame is not null)
				{
					games.Add(dbGame);
				}
			}

			return OperationResult<List<GameModel>>.CreateSuccessfulResponse(games);
		}
		catch (Exception exception)
		{
			return OperationResult<List<GameModel>>.CreateErrorResponse(exception.Message);
		}
	}

	public async ValueTask<OperationResult<List<ElasticGameModel>>> GetSuggestions(Guid guid)
	{
		try
		{
			var purchasedGames = await purchasedGameRepository.SelectByUserId(guid);

			if (purchasedGames is null || purchasedGames.Count is 0)
			{
				return OperationResult<List<ElasticGameModel>>.CreateErrorResponse("No games have been bought yet, nothing to suggest");
			}

			var games = new List<GameModel>();

			var genreDictionary = new Dictionary<Genre, HashSet<string>>();

			foreach (var purchasedGame in purchasedGames)
			{
				var genre = await gameRepository.SelectGenreById(purchasedGame.GameIdentifier);

				if (genre is not Genre.Unknown)
				{
					var gameIdentifierStr = purchasedGame.GameIdentifier.ToString();

					if (!genreDictionary.TryAdd(genre, [gameIdentifierStr]))
					{
						genreDictionary[genre].Add(gameIdentifierStr);
					}
				}
			}

			var suggestedGames = new List<ElasticGameModel>();
			var suggestionsPerGenre = 2;

			foreach (var genre in genreDictionary)
			{
				suggestedGames.AddRange(await elasticService.GetSuggestionsAsync(genre.Key, genre.Value, suggestionsPerGenre));
			}

			return OperationResult<List<ElasticGameModel>>.CreateSuccessfulResponse(suggestedGames);
		}
		catch (Exception exception)
		{
			return OperationResult<List<ElasticGameModel>>.CreateErrorResponse(exception.Message);
		}
	}

	public async ValueTask<OperationResult<IReadOnlyCollection<PopularGame>>> Popular()
	{
		var elasticPopularGames = await elasticPurchasedGameService.SearchPopularGames();

		var popularGames = new List<PopularGame>();

		foreach (var elasticPopularGame in elasticPopularGames)
		{
			var title = await gameRepository.SelectNameById(elasticPopularGame.GameId);

			if (title is null || string.IsNullOrWhiteSpace(title))
			{
				continue;
			}

			popularGames.Add(new PopularGame(elasticPopularGame.GameId, title, elasticPopularGame.PurchaseCount));
		}

		if (popularGames.Count <= 0)
		{
			return OperationResult<IReadOnlyCollection<PopularGame>>.CreateErrorResponse("No games bought to aggregate popular ones");
		}

		return OperationResult<IReadOnlyCollection<PopularGame>>.CreateSuccessfulResponse(popularGames);
	}
}