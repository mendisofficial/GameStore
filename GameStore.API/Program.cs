using GameStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
