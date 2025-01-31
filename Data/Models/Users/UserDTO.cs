/*
    Filename: UserDTO.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users
    File Purpose:
    This file contains the user data transfer object model for inputs/outputs used by 
    endpoints that don't require sensitive data.
    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Users;

public class UserDTO
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int? Role { get; set; }
}