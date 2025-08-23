using Fcg.Game.Api.Documentation;
using Fcg.Game.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSwagger();

var app = builder.Build();
app.AddSwaggerUi();
app.MapGameEndpoints();
app.Run();