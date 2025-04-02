/*
    Filename: CreateStore.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the store object model tied
    to creating a new user.
    
    Class Purpose:
    This record is used as a model for frontend requests
    that create a user.
    
    Written by: Krzysztof Hejno
*/


using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record CreateUser
{
    [Required]
    [StringLength(747, ErrorMessage = "First Name cannot exceed 747 characters.")]
    public required string FIRST_NAME { get; set; }
    [Required]
    [StringLength(747, ErrorMessage = "Last Name cannot exceed 747 characters.")]
    public required string LAST_NAME { get; set; }
    [Required,EmailAddress]
    [StringLength(320, ErrorMessage = "Email cannot exceed 320 characters.")]
    public required string EMAIL { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    public required string PASSWORD { get; set; }
    [Required]
    [Range(1,int.MaxValue, ErrorMessage = "Role must be an integer")]
    public int? ROLE { get; set; }
    [Required]
    public bool ACTIVE { get; set; }
}