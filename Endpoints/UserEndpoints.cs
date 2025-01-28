/*
    Filename: UserEndpoints.cs
    Part of Project: PLOT/PLOT-BE/Endpoints

    File Purpose:
    This file contains the user endpoint mapping.

    Written by: Jordan Houlihan
*/
using Plot.Data.Models;

namespace Plot.Endpoints;

public static class UserEndpoints
{
    private static readonly List<User> sampleUserData = [new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@gmail.com" },
                                new() { Id = 2, FirstName = "Susan", LastName = "Smith", Email = "susansmith@gmail.com" }];

    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllUsers);
        group.MapGet("/{id}", GetUserById);
        group.MapPost("/", CreateUser);
        group.MapDelete("/{id}", DeleteUserById);

        return group;
    }
    static async Task<IResult> GetAllUsers()
    {
        List<UserDTO> sampleGetAllUsers = [];

        foreach (User user in sampleUserData)
        {
            sampleGetAllUsers.Add(new() { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email });
        }

        return TypedResults.Ok(sampleGetAllUsers);
    }

    static async Task<IResult> GetUserById(int id)
    {
        foreach (User user in sampleUserData)
        {
            if (user.Id == id)
            {
                return TypedResults.Ok(new UserDTO { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email });
            }
        }
        return TypedResults.NotFound();
    }

    static async Task<IResult> CreateUser(User newUser)
    {
        sampleUserData.Add(new User
        {
            Id = sampleUserData.Last().Id + 1,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password
        });

        return TypedResults.Ok();
    }

    static async Task<IResult> DeleteUserById(int id)
    {
        foreach (User user in sampleUserData)
        {
            if (user.Id == id)
            {
                sampleUserData.Remove(user);
                return TypedResults.NoContent();
            }
        }
        return TypedResults.NotFound();
    }
}