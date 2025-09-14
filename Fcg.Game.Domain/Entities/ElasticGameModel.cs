using System.Text.Json.Serialization;

namespace Fcg.Game.Domain.Entities
{
	public class ElasticGameModel
	{
		public ElasticGameModel()
		{
			
		}

		public ElasticGameModel(string gameId, string title, string description, int genre, DateTime releaseDateTime, string price)
		{
			Title = title;
			Description = description;
			Genre = genre;
			ReleaseDate = releaseDateTime.Date.ToShortDateString();
			Price = price;
			GameId = gameId;
		}

		public string GameId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int Genre { get; set; }
		public string ReleaseDate { get; set; } = string.Empty;
		public string Price { get; set; } = string.Empty;
	}
}
