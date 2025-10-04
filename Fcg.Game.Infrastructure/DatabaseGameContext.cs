using Fcg.Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Game.Infrastructure;

public class DatabaseGameContext : DbContext
{
	public DbSet<GameModel> Games { get; set; }
	public DbSet<PurchasedGameModel> PurchasedGames { get; set; }

	public DatabaseGameContext() { }

	public DatabaseGameContext(DbContextOptions<DatabaseGameContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseGameContext).Assembly);
	}
}