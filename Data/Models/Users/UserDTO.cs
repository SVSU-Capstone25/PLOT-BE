/*
    Filename: UserDTO.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users
    File Purpose:
    This file contains the DTOs (Data-Transfer Objects)
    used to format the inputs and outputs expected
    from the backend for users.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

/// <summary>
/// This is the output format for
/// a user's public information.
/// </summary>
public record UserDTO
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Role { get; set; }
    public int? Active { get; set; }
}

/// <summary>
/// This is the input format that the backend expects
/// to see when the frontend makes a request
/// to update a user's public information.
/// </summary>
public record UserUpdateDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Role { get; set; }
    public int? Active { get; set; }
}