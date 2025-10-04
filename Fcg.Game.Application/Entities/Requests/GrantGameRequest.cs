namespace Fcg.Game.Application.Entities.Requests;

public class GrantGameRequest
{
	public string PurchaseId { get; set; } = "";
	public string UserId { get; set; } = "";
	public List<GrantGameItem> PurchasedGames { get; set; } = [];
}