using Fcg.Game.Application.Elastic;
using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services;
using Fcg.Game.Application.Services.Ports;
using Fcg.Game.Infrastructure;
using Fcg.Game.Infrastructure.Elastic;
using Fcg.Game.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace Fcg.Game.Api.Setup
{
	public static class ServicesConfiguration
	{
		public static void RegisterServices(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.AddTransient<IGameService, GameService>();
			webApplicationBuilder.Services.AddTransient<IGameRepository, GameRepository>();
			webApplicationBuilder.Services.AddTransient<IPurchasedGameRepository, PurchasedGameRepository>();
			webApplicationBuilder.Services.AddDbContext<DatabaseGameContext>();
		}
	}
}
