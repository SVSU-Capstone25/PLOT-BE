/*
    Filename: UpdateUser.cs
    Part of Project: PLOT/PLOT-BE/Data/Models/Users

    File Purpose:
    This file contains the user object model tied
    to updating a user's public information.

    Class Purpose:
    This record is used as a model for frontend
    requests to update a user's public information.
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UpdatePublicInfoUser
{
    [Required]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "{0} needs to be {2}-{1} characters long.")]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "{0} needs to be {2}-{1} characters long.")]
    public string? LastName { get; set; }
}