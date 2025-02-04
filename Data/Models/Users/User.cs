/*
    Filename: User.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users
    File Purpose:
    This file contains the user object model for inputs/outputs used by 
    endpoints that require sensitive data.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.User;

public class User
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Role { get; set; }
}