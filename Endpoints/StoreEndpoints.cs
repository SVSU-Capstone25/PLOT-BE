/*
    Filename: UserEndpoints.cs
    Part of Project: PLOT/PLOT-BE/Endpoints

    File Purpose:
    This file contains the store endpoint mapping.

    Written by: Jordan Houlihan
*/

namespace Plot.Endpoints;

public static class StoreEndpoints
{
    public static RouteGroupBuilder MapStores(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => { return TypedResults.NoContent(); });
        group.MapGet("/{id}", () => { return TypedResults.NoContent(); });
        group.MapPost("/", () => { return TypedResults.NoContent(); });
        group.MapPut("/{id}", () => { return TypedResults.NoContent(); });
        group.MapDelete("/{id}", () => { return TypedResults.NoContent(); });

        return group;
    }
}
