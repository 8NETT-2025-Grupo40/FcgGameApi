using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Extensions.AWS.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Fcg.Game.Api.Setup
{
	public static class OpenTelemetryConfiguration
	{
		public static void ConfigureOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
		{
			Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator([
				new AWSXRayPropagator(),
				new TraceContextPropagator(),
				new BaggagePropagator()
			]));

			webApplicationBuilder.Services.AddOpenTelemetry()
				.ConfigureResource(r => r.AddService("fcg-game-api"))
				.WithTracing(t => t
					.AddAspNetCoreInstrumentation(o => o.RecordException = true)
					.AddHttpClientInstrumentation()
					.AddEntityFrameworkCoreInstrumentation()
					.AddOtlpExporter());
		}
	}
}
