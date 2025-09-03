namespace Fcg.Game.Application.Elastic
{
	public interface IElasticService<T>
	{
		ValueTask<IReadOnlyCollection<T>> GetAsync(int page, int pageSize);
		ValueTask<IReadOnlyCollection<T>> GetAllAsync();
		ValueTask CreateDocumentAsync(T document);
	}
}