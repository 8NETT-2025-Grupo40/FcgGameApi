using Fcg.Game.Api.Documentation;
using Fcg.Game.Api.Endpoints;
using Fcg.Game.Api.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.RegisterServices();
builder.ConfigureElasticSearch();
builder.AddDbContextConfiguration();

var app = builder.Build();
app.AddSwaggerUi();
app.MapGameEndpoints();
app.Run();