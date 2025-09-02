using Fcg.Game.Application.Repositories;
using Fcg.Game.Application.Services;
using Fcg.Game.Application.Services.Ports;
using Fcg.Game.Infrastructure;
using Fcg.Game.Infrastructure.Repositories;

namespace Fcg.Game.Api.Setup
{
	public static class ServicesConfiguration
	{
		public static void RegisterServices(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.AddTransient<IGameService, GameService>();
			webApplicationBuilder.Services.AddTransient<IElasticService, ElasticService>();
			webApplicationBuilder.Services.AddTransient<IGameRepository, GameRepository>();
			webApplicationBuilder.Services.AddDbContext<DatabaseGameContext>();
		}
	}
}
