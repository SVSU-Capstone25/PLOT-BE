/*
    Filename: AccessModel.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the request input for adding
    or deleting a user to a store.
    
    Written by: Josh Rodack
*/



using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record AccessModel
{
    [Required]
    public required int USER_TUID { get; set; }
    [Required]
    public required int STORE_TUID { get; set; }
}