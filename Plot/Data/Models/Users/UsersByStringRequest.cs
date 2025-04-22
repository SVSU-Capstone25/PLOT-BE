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

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UsersByStringRequest
{
    [Required]
    public required string TUIDS { get; set; }
}