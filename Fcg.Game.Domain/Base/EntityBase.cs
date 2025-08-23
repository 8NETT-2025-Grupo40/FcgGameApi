namespace Fcg.GameDomain.Base;

public abstract class EntityBase
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public StatusBase Status { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateUpdated { get; set; }
}