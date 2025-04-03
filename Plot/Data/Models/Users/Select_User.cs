/*
    Filename: Select_User.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the record used for 
    mapping user information from requests
    and responses.

    Class Purpose:
    This record will be used to map inputs/outputs
    for the frontend/database.
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record Select_User
{
    public int? TUID { get; set; }
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public required string FIRST_NAME { get; set; }
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public required string LAST_NAME { get; set; }
    [EmailAddress]
    [StringLength(320, ErrorMessage = "First Name cannot exceed 320 characters.")]
    public required string EMAIL { get; set; }
    [StringLength(10, ErrorMessage = "Role cannot exceed 10 characters.")]
    public required string ROLE { get; set; }
    public bool? ACTIVE { get; set; }
}