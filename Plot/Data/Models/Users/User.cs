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
    public int? TUID { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? LAST_NAME { get; set; }
    public string? EMAIL { get; set; }
    public string? PASSWORD { get; set; }
    public string? ROLE { get; set; }
    public bool ACTIVE { get; set; }
}