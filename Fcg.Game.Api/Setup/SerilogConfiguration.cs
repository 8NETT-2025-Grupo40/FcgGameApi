using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;

namespace Fcg.Game.Api.Setup
{
	public static class SerilogConfiguration
	{
		public static void ConfigureSerilog(this WebApplicationBuilder webApplicationBuilder)
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(webApplicationBuilder.Configuration)
				.Enrich.FromLogContext()
				.Enrich.WithSpan()
				.WriteTo.Console(new RenderedCompactJsonFormatter())
				.CreateLogger();

			webApplicationBuilder.Host.UseSerilog();
		}
	}
}