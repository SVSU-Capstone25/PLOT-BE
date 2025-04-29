/*
    Filename: UsersByStringRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the users by string request model.
    
    Class Purpose:
    This record is used as a model the input
    for a get users by string request. The TUIDS
    variable will be used as a string of user ids.
    
    Written by: I have no idea who wrote this but they
    copy and pasted my comments from User.cs on here so I'm changing the comments - (Jordan Houlihan)
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UsersByStringRequest
{
    [Required]
    public required string TUIDS { get; set; }
}