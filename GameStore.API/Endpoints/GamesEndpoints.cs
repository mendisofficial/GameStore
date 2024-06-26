﻿using GameStore.API.Contracts;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";

    private static readonly List<GameDTO> games = [
        new (
            Id: 1,
            Name: "The Legend of Zelda: Breath of the Wild",
            Genre: "Action-adventure",
            Price: 59.99m,
            ReleaseDate: new DateOnly(2017, 3, 3)
        ),
        new (
            Id: 2,
            Name: "Super Mario Odyssey",
            Genre: "Platformer",
            Price: 59.99m,
            ReleaseDate: new DateOnly(2017, 10, 27)
        ),
        new (
            Id: 3,
            Name: "Mario Kart 8 Deluxe",
            Genre: "Racing",
            Price: 59.99m,
            ReleaseDate: new DateOnly(2017, 4, 28)
        )
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id} 
        group.MapGet("/{id}", (int id) =>
        {
            GameDTO? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndPointName);

        // POST /games
        group.MapPost("/", (CreateGameDTO newGame) =>
        {
            GameDTO game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDTO(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
