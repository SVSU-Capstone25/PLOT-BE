/*
    Filename: Users.cs
    Part of Project: PLOT/PLOT-BE
    File Purpose:
    This file contains the model/model bindings for users.
    Written by: Jordan Houlihan
*/

using System.ComponentModel.DataAnnotations;

namespace Plot;

public class User
{

    public int? Id { get; set; }
    [Required]
    [MaxLength(747)]
    public string? FirstName { get; set; }
    [Required]
    [MaxLength(747)]
    public string? LastName { get; set; }
    [Required]
    [MaxLength(320)]
    public string? Email { get; set; }
    [Required]
    public int? Role { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Password { get; set; }
}

public class UserLoginDTO
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}