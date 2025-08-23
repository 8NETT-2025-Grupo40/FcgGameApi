using Microsoft.OpenApi.Models;

namespace Fcg.Game.Api.Documentation
{
	public static class SwaggerConfiguration
	{
		private const string API_VERSION = "v1";
		private const string API_NAME = $"Fcg.Game.Api {API_VERSION}";
		private const string SWAGGER_PATH = "/swagger/v1/swagger.json";

		public static void ConfigureSwagger(this WebApplicationBuilder webApplicationBuilder)
		{
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = API_NAME,
				Version = API_VERSION
			}));
		}

		public static void AddSwaggerUi(this WebApplication webApplication)
		{
			if (webApplication.Environment.IsDevelopment())
			{
				webApplication.UseSwagger();

				webApplication.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint(SWAGGER_PATH, API_NAME);
					options.RoutePrefix = string.Empty;
				});
			}
		}
	}
}