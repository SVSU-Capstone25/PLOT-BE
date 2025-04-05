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

public record User
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "TUID must be an integer")]
    public int? TUID { get; set; }
    [Required]
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public string? FIRST_NAME { get; set; }
    [Required]
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public string? LAST_NAME { get; set; }
    [Required, EmailAddress]
    [StringLength(320, ErrorMessage = "First Name cannot exceed 320 characters.")]
    public string? EMAIL { get; set; }
    [Required]
    [StringLength(320, ErrorMessage = "Password cannot exceed 320 characters.")]
    public string? PASSWORD { get; set; }
    [Required]
    [StringLength(10, ErrorMessage = "Role cannot exceed 10 characters.")]
    public string? ROLE { get; set; }
    [Required]
    public bool ACTIVE { get; set; }
}