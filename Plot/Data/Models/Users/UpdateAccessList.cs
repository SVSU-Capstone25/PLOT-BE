/*
    Filename: UpdateAccessList.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the object to hold the access attributes
    for a full update on a user's list of stores.
    
    Class Purpose:
    This record is used as the file for completely
    updating a user's list of stores.
    
    Written by: Josh Rodack
*/



using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UpdateAccessList
{
    [Required]
    public required int USER_TUID { get; set; }
    [Required]
    public required IEnumerable<int> STORE_TUIDS { get; set; }
}