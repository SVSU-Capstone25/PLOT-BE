/*
    Filename: UpdateUser.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users

    File Purpose:
    This file contains the user object model tied
    to updating a user's public information.

    Class Purpose:
    This record is used as a model for frontend
    requests to update a user's public information.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

public record UpdateUser
{
    public int? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Role { get; set; }
    public int? Active { get; set; }
}