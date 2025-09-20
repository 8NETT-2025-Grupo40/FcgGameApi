namespace Fcg.Game.Application.Elastic
{
	public interface IElasticService<T>
	{
		ValueTask<IReadOnlyCollection<T>> GetAllAsync();
		ValueTask CreateDocumentAsync(T document);
	}
}