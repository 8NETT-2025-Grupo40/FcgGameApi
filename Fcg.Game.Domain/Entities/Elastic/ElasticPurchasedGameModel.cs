using System.Text.Json.Serialization;

namespace Fcg.Game.Domain.Entities.Elastic
{
	public class ElasticPurchasedGameModel
	{
		[JsonPropertyName("UserIdentifier")]
		public string UserIdentifier { get; set; } = string.Empty;

		[JsonPropertyName("GameIdentifier")]
		public string GameIdentifier { get; set; } = string.Empty;
	}
}
