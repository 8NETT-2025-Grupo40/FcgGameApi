using System.Text.Json.Serialization;

namespace Fcg.Game.Domain.Entities
{
	public class PopularGame(Guid id, string name, long purchaseCount)
	{
		[JsonPropertyName("GameIdentifier")]
		public Guid Id { get; set; } = id;
		
		[JsonPropertyName("Name")]
		public string Name { get; set; } = name;
		
		[JsonPropertyName("PurchaseCount")]
		public long PurchaseCount { get; set; } = purchaseCount;

		public override string ToString() => $"{Name}\nPurchased {PurchaseCount} times.";
	}
}
