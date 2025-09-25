using Fcg.Game.Application.Elastic;
using Fcg.Game.Infrastructure.Elastic;
using Microsoft.Extensions.Options;

namespace Fcg.Game.Api.Setup
{
	public static class ElasticConfiguration
	{
		public static void ConfigureElasticSearch(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.Configure<ElasticSettings>(webApplicationBuilder.Configuration.GetSection(nameof(ElasticSettings)));

			webApplicationBuilder.Services.AddSingleton<IElasticSettings>(serviceProvider =>
				serviceProvider.GetRequiredService<IOptions<ElasticSettings>>().Value);

			webApplicationBuilder.Services.AddSingleton(typeof(IElasticService<>), typeof(ElasticService<>));
			webApplicationBuilder.Services.AddSingleton<IElasticGameService, ElasticGameService>();
			webApplicationBuilder.Services.AddSingleton<IElasticPurchasedGameService, ElasticPurchasedGameService>();
		}
	}
}
