using Fcg.Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fcg.Game.Infrastructure.Configurations;

public class PurchasedGameConfiguration : IEntityTypeConfiguration<PurchasedGameModel>
{
	public void Configure(EntityTypeBuilder<PurchasedGameModel> purchasedGameBuilder)
	{
		// Entity Base Properties
		purchasedGameBuilder.HasKey(e => e.Id);
		purchasedGameBuilder.Property(p => p.Id).IsRequired();

		purchasedGameBuilder.Property(p => p.DateCreated).IsRequired();
		purchasedGameBuilder.Property(p => p.DateUpdated);
		purchasedGameBuilder.Property(p => p.Status).IsRequired();

		// Pruchased Game Specific Properties
		purchasedGameBuilder.ToTable("PurchasedGames");

		purchasedGameBuilder.Property(p => p.UserIdentifier).IsRequired();
		purchasedGameBuilder.Property(p => p.GameIdentifier).IsRequired();
	}
}
