using Fcg.Game.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;

namespace Fcg.Game.Api.HealthChecks
{
	public class DatabaseHealthCheck<TContext>(
		TContext databaseContext) : IHealthCheck where TContext : DatabaseGameContext
	{
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			try
			{
				await databaseContext.Database.ExecuteSqlAsync($"SELECT 1", cancellationToken);
				return HealthCheckResult.Healthy();
			}
			catch (DbException dbEx)
			{
				return HealthCheckResult.Unhealthy(
					description: $"Failed to connect in database: {dbEx.Message}",
					exception: dbEx);
			}
			catch (Exception ex)
			{
				return HealthCheckResult.Unhealthy(
					description: $"Unexpected error: {ex.Message}",
					exception: ex);
			}
		}
	}
}