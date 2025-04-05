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
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public string? FIRST_NAME { get; set; }
    [Required]
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public string? LAST_NAME { get; set; }

    [Required, EmailAddress]
    [StringLength(320, ErrorMessage = "First Name cannot exceed 320 characters.")]
    public string? EMAIL { get; set; }
}