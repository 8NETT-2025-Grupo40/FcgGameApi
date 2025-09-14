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

		[JsonPropertyName("GameId")]
		public string GameId { get; set; }
		
		[JsonPropertyName("Title")]
		public string Title { get; set; } = string.Empty;
		
		[JsonPropertyName("Description")]
		public string Description { get; set; } = string.Empty;
		
		[JsonPropertyName("Genre")]
		public int Genre { get; set; }
		
		[JsonPropertyName("ReleaseDate")]
		public string ReleaseDate { get; set; } = string.Empty;
		
		[JsonPropertyName("Price")]
		public string Price { get; set; } = string.Empty;
	}
}
