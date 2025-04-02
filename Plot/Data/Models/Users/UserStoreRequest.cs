/*
    Filename: UserStoreRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the object to hold the Access attributes.
    
    Class Purpose:
    This record is used as the file 
    to pass back the values to remove or add
    a user from a store.
    
    Written by: Josh Rodack
*/



using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UserStoreRequest
{
    [Required]
    [Range(1,int.MaxValue, ErrorMessage = "RoleId must be an integer")]
    public int USER_TUID { get; set; }
    [Required]
    [Range(1,int.MaxValue, ErrorMessage = "storeId must be an integer")]
    public int STORE_TUID { get; set; }
}