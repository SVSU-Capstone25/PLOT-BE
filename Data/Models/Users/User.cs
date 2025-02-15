/*
    Filename: User.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users
    File Purpose:
    This file contains the user object model.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

public class User
{
    public int? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Role { get; set; }
    public int? Active { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}