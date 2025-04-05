/*
    Filename: UserDTO.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the DTO (Data-Transfer Object)
    used to format the inputs and outputs expected
    from the backend for users.

    Class Purpose:
    This record is used as a way to transfer user
    data without transferring the sensitive data as
    well.
    
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot.Data.Models.Users;

public record UserDTO
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
    [StringLength(10, ErrorMessage = "Role cannot exceed 10 characters.")]
    public string? ROLE { get; set; }
}