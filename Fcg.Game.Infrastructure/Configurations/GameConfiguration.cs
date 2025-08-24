using Fcg.Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fcg.Game.Infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<GameModel>
{
	public void Configure(EntityTypeBuilder<GameModel> gameBuilder)
	{
		// Entity Base Properties
		gameBuilder.HasKey(e => e.Id);
		gameBuilder.Property(p => p.Id).IsRequired();

		gameBuilder.Property(p => p.DateCreated).IsRequired();
		gameBuilder.Property(p => p.DateUpdated);
		gameBuilder.Property(p => p.Status).IsRequired();

		// Game Specific Properties
		gameBuilder.ToTable("Game");

		gameBuilder.OwnsOne(g => g.Title, title =>
		{
			title.Property(t => t.Value).HasMaxLength(255).HasColumnName("Title").IsRequired();
		});

		gameBuilder.OwnsOne(g => g.Description, description =>
		{
			description.Property(d => d.Value).HasMaxLength(510).HasColumnName("Description").IsRequired();
		});

		gameBuilder.Property(g => g.Genre)
			.IsRequired();

		gameBuilder.Property(g => g.ReleaseDate)
			.IsRequired();

		gameBuilder.OwnsOne(g => g.Price, price =>
		{
			price.Property(p => p.Value)
				.HasColumnName("Price")
				.HasPrecision(10, 2)
				.IsRequired();

			price.WithOwner();
		});

		//gameBuilder.HasMany(g => g.PlayerProfiles)
		//	.WithOne(pg => pg.Game)
		//	.HasForeignKey(pg => pg.GameId);
	}
}