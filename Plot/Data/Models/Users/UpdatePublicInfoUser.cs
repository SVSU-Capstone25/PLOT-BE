/*
    Filename: UpdatePublicInfoUser.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

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
    public string? FIRST_NAME { get; set; }

    [Required]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "{0} needs to be {2}-{1} characters long.")]
    public string? LAST_NAME { get; set; }

    public string? EMAIL { get; set; }

    public string? ROLE { get; set; }
    public bool? ACTIVE { get; set; }
    public string? STORENAME { get; set;}
}