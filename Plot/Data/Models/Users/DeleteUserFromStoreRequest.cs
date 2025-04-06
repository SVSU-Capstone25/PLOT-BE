/*
    Filename: DeleteUserFromStoreRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the object to hold the Access attributes.
    
    Class Purpose:
    This record is used as the file 
    to pass back the values to remove
    a user from a store.
    
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