namespace Fcg.Game.Api.Endpoints
{
	public static class GameEndpoints
	{
		public static void MapGameEndpoints(this WebApplication webApplication)
		{
			webApplication
				.MapGroup("games")
				.WithTags("Game");

			webApplication.MapGet("/health", Get);
		}

		private static string Get() => "Healthy: This is the game api!";
	}
}