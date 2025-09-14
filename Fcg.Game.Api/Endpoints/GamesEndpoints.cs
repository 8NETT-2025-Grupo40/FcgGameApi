using Fcg.Game.Application.Entities.Requests;
using Fcg.Game.Application.Services.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Fcg.Game.Api.Endpoints
{
	public static class GamesEndpoints
	{
		public static void MapGameEndpoints(this WebApplication webApplication)
		{
			webApplication
				.MapGroup("games")
				.WithTags("Game");

			webApplication.MapGet("/sample", Sample);
			webApplication.MapPost("/games", CreateGame).WithName("Create a new game");
			webApplication.MapGet("/games", ListGames).WithName("Get list of available games");
			webApplication.MapPost("/library", GrantGameToUser).WithName("Confirm a game purchase");
			webApplication.MapGet("/library", RetriveLibraryByUserId).WithName("Retrieve user's library");
			webApplication.MapGet("/suggestions", SuggestGames).WithName("Suggest games based on user's favorite genres");
		}

		private static string Sample() => "Healthy: This is the game api!";

		private static async ValueTask<IResult> CreateGame(
			IGameService gameService,
			[FromBody] CreateGameRequest createGameRequest)
		{
			var operationResult = await gameService.CreateGame(createGameRequest);

			if (operationResult.IsSuccessful is false)
			{
				return Results.BadRequest(operationResult.Message);
			}

			return Results.Ok(operationResult.Value.ToString());
		}

		private static async ValueTask<IResult> ListGames(
			IGameService gameService)
		{
			var operationResult = await gameService.ListAvailableGames();

			if (operationResult.IsSuccessful is false)
			{
				return Results.BadRequest(operationResult.Message);
			}

			return Results.Ok(operationResult.Value);
		}

		private static async ValueTask<IResult> GrantGameToUser(
			IGameService gameService,
			[FromBody] GrantGameRequest grantGameRequest)
		{
			var operationResult = await gameService.AddGameToLibrary(grantGameRequest);

			if (operationResult.IsSuccessful is false)
			{
				return Results.BadRequest(operationResult.Message);
			}

			return Results.Ok(operationResult.Message);
		}

		private static async ValueTask<IResult> RetriveLibraryByUserId(
			IGameService gameService,
			[FromQuery] Guid userId)
		{
			var operationResult = await gameService.RetrieveGamesById(userId);

			if (operationResult.IsSuccessful is false)
			{
				return Results.BadRequest(operationResult.Message);
			}

			return Results.Ok(operationResult.Value);
		}

		private static async ValueTask<IResult> SuggestGames(
			IGameService gameService,
			[FromQuery] Guid userId)
		{
			var operationResult = await gameService.GetSuggestions(userId);

			if (operationResult.IsSuccessful is false)
			{
				return Results.BadRequest(operationResult.Message);
			}

			return Results.Ok(operationResult.Value);
		}
	}
}