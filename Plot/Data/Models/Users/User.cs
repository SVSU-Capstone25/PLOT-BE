/*
    Filename: User.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the user object model.
    
    Class Purpose:
    This record is used as the main model
    for users in the application. This will
    look the same as the schema in the database.
    
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

public record User
{
    public int? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public bool Active { get; set; }
}