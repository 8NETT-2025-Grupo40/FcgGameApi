using Fcg.Game.Api.HealthChecks;
using Fcg.Game.Infrastructure;

namespace Fcg.Game.Api.Setup
{
	public static class HealthCheckConfiguration
	{
		public static void ConfigureHealthChecks(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck<DatabaseGameContext>>("DbContext_Check");
		}
	}
}