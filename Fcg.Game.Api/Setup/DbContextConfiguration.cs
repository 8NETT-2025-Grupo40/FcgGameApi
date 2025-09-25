using Fcg.Game.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Game.Api.Setup
{
	public static class DbContextConfiguration
	{
		public static void AddDbContextConfiguration(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.AddDbContext<DatabaseGameContext>(options =>
			{
				string? connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");

				if (string.IsNullOrWhiteSpace(connectionString))
				{
					throw new InvalidOperationException("Could find connection string, database will not be configured");
				}

				options.UseSqlServer(connectionString);
			}, ServiceLifetime.Scoped);
		}
	}
}